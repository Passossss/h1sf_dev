using H1SF.Domain.Entities.Faturamento;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services;

/// <summary>
/// 570-00-RETRIEVE-PARAMETRO - Recupera e valida parâmetros da transação COBOL
/// Linhas COBOL: 4427-4462
/// </summary>
public class ProcessadorFaturamento : IProcessadorFaturamento
{
    private readonly ILogger<ProcessadorFaturamento> _logger;
    
    public ProcessadorFaturamento(ILogger<ProcessadorFaturamento> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// 570-00-RETRIEVE-PARAMETRO SECTION
    /// Recebe string de 24 bytes e valida parâmetros
    /// </summary>
    public FaturamentoParametros RetrieveParametro(string dadosRecebidos)
    {
        _logger.LogDebug("570-00-RETRIEVE-PARAMETRO iniciado");
        
        // MOVE 24 TO WS33-UND-TAM-PARM + validação
        const int tamanhoEsperado = 24;
        if (string.IsNullOrEmpty(dadosRecebidos) || dadosRecebidos.Length != tamanhoEsperado)
        {
            throw new ArgumentException(
                $"Tamanho inválido. Esperado {tamanhoEsperado} bytes, recebido: {dadosRecebidos?.Length ?? 0}");
        }
        
        // EXEC CICS RETRIEVE INTO (WS36-ARE-RECEIVE) - extrai campos conforme layout
        var parametros = new FaturamentoParametros
        {
            CodigoMercadoriaDestino = dadosRecebidos[0],        // PIC X(001) - byte 0
            DataHoraSelecao = dadosRecebidos.Substring(1, 14),  // PIC X(014) - bytes 1-14
            LoginFuncionario = dadosRecebidos.Substring(15, 8), // PIC X(008) - bytes 15-22
            FaseFaturamento = dadosRecebidos[23]                // PIC X(001) - byte 23
        };
        
        // IF WS36-CD-MERC-DST NOT GREATER SPACES
        if (parametros.CodigoMercadoriaDestino == ' ' || parametros.CodigoMercadoriaDestino == '\0')
        {
            var msgErro = $"*** ERRO - PARAMETRO P/ SF33 INVALIDO   ===> {dadosRecebidos}";
            _logger.LogError("H1SF0033: {Mensagem}", msgErro);
            throw new InvalidOperationException(msgErro);
        }
        
        _logger.LogDebug(
            "Parâmetros validados: {CodigoMercadoriaDestino}|{DataHoraSelecao}|{LoginFuncionario}|{FaseFaturamento}",
            parametros.CodigoMercadoriaDestino,
            parametros.DataHoraSelecao,
            parametros.LoginFuncionario.Trim(),
            parametros.FaseFaturamento);
        
        return parametros;
    }
}
