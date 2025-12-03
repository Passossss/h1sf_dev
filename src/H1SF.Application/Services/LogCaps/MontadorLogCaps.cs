using H1SF.Domain.Entities.LogCaps;
using H1SF.Infrastructure.Repositories.LogCaps;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.LogCaps;

/// <summary>
/// 875-00-MONTA-LOG-CAPS - Montagem de log para sincronização CAPS
/// Linhas COBOL: aproximadamente 13000-13300
/// Autor: E. FRIOLI JR.
/// </summary>
public class MontadorLogCaps : IMontadorLogCaps
{
    private readonly ILogCapsRepository _repository;
    private readonly ILogger<MontadorLogCaps> _logger;

    public MontadorLogCaps(
        ILogCapsRepository repository,
        ILogger<MontadorLogCaps> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 875-00-MONTA-LOG-CAPS SECTION
    /// Condição de execução: WS36-CD-MERC-DST='E' AND WS36-FASE-FTRM='1' AND (ST0001-CD-REGR-FTRM='G' OR 'F')
    /// </summary>
    public async Task MontarLogCapsAsync(MontarLogCapsInputDto input)
    {
        _logger.LogDebug("875-00-MONTA-LOG-CAPS iniciado");

        try
        {
            int idSequenciaMq = 0;

            // 875-10-RECUPERA-TOTAL
            var (codigoFornecedor, totalSelecao) = await RecuperarTotalAsync(input);

            // Obtém data/hora do sistema para processamento
            var sysdate = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 875-20-RECUPERA-DETALHE
            var detalhes = await _repository.RecuperarDetalhesItensAsync(
                input.CodigoMercadoDestino,
                input.DataSelecaoFaturamento,
                input.LoginFuncionario);

            _logger.LogInformation(
                "875-00-MONTA-LOG-CAPS: Processando {Count} itens para fornecedor {Fornecedor}",
                detalhes.Count, codigoFornecedor);

            // 875-20-LOOP-DETALHE
            foreach (var detalhe in detalhes)
            {
                idSequenciaMq++;
                
                // 875-30-MOVIMENTA-LOG
                var logDto = MontarLayoutLogCaps(
                    idSequenciaMq,
                    codigoFornecedor,
                    totalSelecao,
                    sysdate,
                    detalhe,
                    input);

                // 875-40-GRAVA-LOG
                await GravarLogMqAsync(logDto, input);
            }

            // 875-60-LOG-START-CAPS
            idSequenciaMq++;
            await GravarLogStartCapsAsync(
                idSequenciaMq,
                sysdate,
                detalhes.FirstOrDefault()?.DataSelecaoFaturamento ?? string.Empty,
                input);

            _logger.LogDebug("875-00-MONTA-LOG-CAPS concluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao montar log CAPS");
            throw;
        }
    }

    /// <summary>
    /// 875-10-RECUPERA-TOTAL
    /// Busca código fornecedor e total da seleção
    /// </summary>
    private async Task<(string CodigoFornecedor, decimal TotalSelecao)> RecuperarTotalAsync(
        MontarLogCapsInputDto input)
    {
        var resultado = await _repository.RecuperarTotalSelecaoAsync(
            input.CodigoMercadoDestino,
            input.DataSelecaoFaturamento,
            input.LoginFuncionario,
            input.FaseFaturamento);

        _logger.LogDebug(
            "Total seleção recuperado: Fornecedor={Fornecedor}, Total={Total}",
            resultado.CodigoFornecedor, resultado.TotalSelecao);

        return resultado;
    }

    /// <summary>
    /// 875-30-MOVIMENTA-LOG
    /// Monta layout SF80042-ASN-LAYOUT para registro tipo 'DN' (detalhe)
    /// </summary>
    private LogCapsDto MontarLayoutLogCaps(
        int idSequenciaMq,
        string codigoFornecedor,
        decimal totalSelecao,
        string sysdate,
        LogCapsDetalheDto detalhe,
        MontarLogCapsInputDto input)
    {
        var logDto = new LogCapsDto
        {
            IdSequencia = idSequenciaMq,
            TipoRegistro = "DN", // Registro de detalhe
            CodigoAcesso = "02",
            
            // Dados do cabeçalho
            CodigoFabricaRecebimento = "ZQ",
            CodigoFornecedor = codigoFornecedor,
            TotalFatura = totalSelecao,
            CodigoMoeda = "USD",
            
            // Data da fatura (ITD_DTC_FTR_EXP)
            NumeroFatura = detalhe.NumeroFaturaExportacao
        };

        // Processar data da fatura: YYYYMMDDHH24MISS -> CC/YY/MMDD
        if (detalhe.DataFaturaExportacao.Length >= 8)
        {
            var ano = detalhe.DataFaturaExportacao.Substring(2, 2); // YY
            var mesDia = detalhe.DataFaturaExportacao.Substring(4, 4); // MMDD
            
            logDto.DataFaturaCenturiaCC3 = "20";
            logDto.DataFaturaAnoYY3 = ano;
            logDto.DataFaturaMesDiaMmdd3 = mesDia;
        }

        // Data de processamento (SYSDATE)
        if (sysdate.Length >= 8)
        {
            var ano = sysdate.Substring(2, 2); // YY
            var mesDia = sysdate.Substring(4, 4); // MMDD
            
            logDto.DataProcessamentoCenturiaCC4 = "20";
            logDto.DataProcessamentoAnoYY4 = ano;
            logDto.DataProcessamentoMesDiaMmdd4 = mesDia;
        }

        // Dados da linha de item
        logDto.ReferenciaPedido = detalhe.NumeroPedido;
        logDto.ReferenciaCliente = detalhe.ReferenciaCliente;
        logDto.TipoLinhaFatura = "001";
        logDto.CodigoCatalogo = detalhe.CodigoPeca;
        logDto.QuantidadeFaturada = detalhe.QuantidadeFaturada;
        logDto.UnidadeMedidaPreco = 3;
        logDto.PrecoUnitario = detalhe.PrecoUnitario;
        logDto.PrecoTotal = detalhe.PrecoTotal;
        logDto.DescricaoPeca = detalhe.NomePecaIngles;
        logDto.NumeroControleIntercambio = detalhe.ReferenciaCliente;

        // Monta ID de correlação da mensagem
        // MOVE SF0002-ITD-DTC-SEL-FTRM TO WS35-ID-CORR-ID-LIT-SC
        // MOVE CC0001-NUMERO-CTB-5 TO WS35-ID-CORR-ID-ALF-SC
        // INSPECT WS35-ID-CORREL-ID CONVERTING SPACES TO ZEROS
        var idCorrelacao = $"{detalhe.DataSelecaoFaturamento}{input.NumeroContabil}".Replace(" ", "0");
        logDto.IdCorrelacaoMensagem = idCorrelacao;
        
        logDto.ChavePadrao = logDto.TipoRegistro;
        logDto.TamanhoMensagem = 461;

        return logDto;
    }

    /// <summary>
    /// 875-40-GRAVA-LOG
    /// Grava mensagem tipo 'DN' na fila MQ
    /// </summary>
    private async Task GravarLogMqAsync(LogCapsDto logDto, MontarLogCapsInputDto input)
    {
        // PERFORM 555-00-GRAVA-MENSAGEM-MQ
        // Esta parte requer integração com IBM MQ (WebSphere MQ)
        // Por enquanto, registramos em log
        
        _logger.LogInformation(
            "875-40-GRAVA-LOG: Tipo={Tipo}, Seq={Seq}, Fila=00111, TamMsg={Tam}, ChavePadrao={Chave}",
            logDto.TipoRegistro,
            logDto.IdSequencia,
            logDto.TamanhoMensagem,
            logDto.ChavePadrao);

        // TODO: Implementar gravação real na fila MQ quando disponível
        // ACB50221-ID-FILA-MQ = 00111
        // ACB50221-TAM-MSG = 461
        // ACB50221-TXT-MSG = SF80042-ASN-LAYOUT
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// 875-60-LOG-START-CAPS
    /// Grava registro tipo 'ST' (Start) na fila MQ
    /// </summary>
    private async Task GravarLogStartCapsAsync(
        int idSequenciaMq,
        string sysdate,
        string dataSelecaoFaturamento,
        MontarLogCapsInputDto input)
    {
        var logStart = new LogCapsDto
        {
            IdSequencia = idSequenciaMq,
            TipoRegistro = "ST", // Registro de START
            CodigoAcesso = "01",
            TamanhoMensagem = 461
        };

        // Data de processamento
        if (sysdate.Length >= 8)
        {
            var ano = sysdate.Substring(2, 2);
            var mesDia = sysdate.Substring(4, 4);
            
            logStart.DataProcessamentoCenturiaCC4 = "20";
            logStart.DataProcessamentoAnoYY4 = ano;
            logStart.DataProcessamentoMesDiaMmdd4 = mesDia;
        }

        // ID de correlação
        var idCorrelacao = $"{dataSelecaoFaturamento}{input.NumeroContabil}".Replace(" ", "0");
        logStart.IdCorrelacaoMensagem = idCorrelacao;
        logStart.ChavePadrao = logStart.TipoRegistro;

        _logger.LogInformation(
            "875-60-LOG-START-CAPS: Tipo={Tipo}, Seq={Seq}, Fila=00111",
            logStart.TipoRegistro,
            logStart.IdSequencia);

        // TODO: Implementar gravação real na fila MQ
        await Task.CompletedTask;
    }
}
