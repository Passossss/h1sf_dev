using H1SF.Application.DTOs.Interface;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.InterfaceCmeItem855;

/// <summary>
/// Implementação para 855-00-INTERFACE-CME-ITEM
/// Envia dados do item para o sistema CME via MQ
/// </summary>
public class InterfaceCmeItem855Service : IInterfaceCmeItem855
{
    private readonly ApplicationDbContext _context;

    public InterfaceCmeItem855Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterfaceCmeItem855Output> ExecutarAsync(InterfaceCmeItem855Input input)
    {
        var output = new InterfaceCmeItem855Output();

        try
        {
            //mock MOVE '00-02' TO SE8-CD-EMP
            string codigoEmpresa = "00-02";

            //mock MOVE SF0002-ITD-DTC-SEL-FTRM TO WS01-DATA-AAAAMMDD
            string dataAaaaMmDd = input.DataSelecaoFaturamento.ToString("yyyyMMdd");

            //mock MOVE CC0001-NUMERO-CTB-5 TO WS01-NUMERO-CTB
            string numeroContabil = input.NumeroNotaContabil5 + input.NumeroNotaContabil6;

            //mock MOVE ST0006-ID-FRN TO WS01-ID-FRN
            int idFornecedor = input.IdFornecedor;

            //mock MOVE CC0002-IDF-NUM-4 TO WS01-IDF-NUM
            string idfNum = input.IdentificacaoNumero4;

            //mock MOVE FUNCTION CURRENT-DATE TO WS31-CURRENT-DATE
            DateTime dataAtual = DateTime.Now;

            //mock MOVE WS32-CURRENT-DATE TO WS01-DATA-GERACAO
            string dataGeracao = dataAtual.ToString("yyyyMMdd");

            //mock MOVE WS32-CURRENT-TIME TO WS01-HORA-GERACAO
            string horaGeracao = dataAtual.ToString("HHmmss");

            //mock MOVE WS36-CD-SIS-ORIG-CME TO WS01-ID-SIS-ORI
            string idSistemaOrigem = "SF00";

            //mock código de acesso (5 posições aleatórias)
            string codigoAcesso = new Random().Next(10000, 99999).ToString();

            //mock MOVE WS01-ID-CORR TO SE8-ID-CORR-ALF-17
            string idCorrelacao = $"{dataAaaaMmDd}{numeroContabil}{idfNum}";

            //mock INSPECT SE8-ID-CORR-ALF-17 CONVERTING ' ' TO '0'
            idCorrelacao = idCorrelacao.Replace(" ", "0");

            //mock MOVE 'E' TO SE8-CD-TP-MOV
            string tipoMovimento = "E";

            //mock MOVE SE8-CD-EMP TO SE8-CD-NTA-FSCL
            string codigoNotaFiscal = codigoEmpresa;

            //mock MOVE ST0006-ID-FRN TO SE8-CD-FRN
            string codigoFornecedor = idFornecedor.ToString().PadLeft(15, '0');

            //mock MOVE CC0002-PRECO-UNITARIO-CME TO SE8-V-UNIT-X
            decimal valorUnitario = input.PrecoUnitarioCme;

            //mock INSPECT SE8-V-UNIT-X CONVERTING ',' TO '.'
            //INSPECT SE8-V-UNIT-X CONVERTING ' ' TO '0'

            //mock MOVE SF0002-ITD-ID-PECA-LOG TO SE8-CD-PSO
            string codigoPso = input.IdPeca;

            //mock INSPECT SE8-CD-PSO CONVERTING ' ' TO '0'
            codigoPso = codigoPso.Replace(" ", "0");

            //mock MOVE SF0002-ITD-ID-PDD-LOG TO SE8-CD-IDF-PDD
            string codigoIdfPdd = input.IdPdd;

            //mock INSPECT SE8-CD-IDF-PDD CONVERTING ' ' TO '0'
            codigoIdfPdd = codigoIdfPdd.Replace(" ", "0");

            //mock MOVE CC0002-ALIQ-ICMS-LOG TO SE8-P-ICMS-X
            decimal percentualIcms = input.AliquotaIcms;

            //mock MOVE CC0002-ALIQ-IPI-LOG TO SE8-P-IPI-X
            decimal percentualIpi = input.AliquotaIpi;

            //mock MOVE CC0002-VL-ICMS-CME TO SE8-V-ICMS-X
            decimal valorIcms = input.ValorIcmsCme;

            //mock MOVE CC0002-VL-IPI-CME TO SE8-V-IPI-X
            decimal valorIpi = input.ValorIpiCme;

            //mock MOVE SF0002-ITD-Q-PECA-FTRD TO SE8-Q-ITEM
            int quantidadeItem = input.QuantidadePecaFaturada;

            //mock MOVE CC0001-DH-EMISSAO-BARRA TO SE8-DT-DH-EMIS
            string dataEmissao = input.DataEmissaoBarras;

            //mock MOVE 896 TO WQ01-TAM-MSG-SDE
            int tamanhoMensagem = 896;

            //mock PERFORM 555-00-GRAVA-MENSAGEM-MQ (envio para fila MQ 00045)
            //mock envia SF80032 para fila 00045
            bool mensagemEnviada = true;

            output.Sucesso = true;
            output.CodigoAcesso = codigoAcesso;
            output.IdCorrelacao = idCorrelacao;
            output.CodigoNotaFiscal = codigoNotaFiscal;
            output.CodigoFornecedor = codigoFornecedor;
            output.TipoMovimento = tipoMovimento;
            output.TamanhoMensagem = tamanhoMensagem;
            output.MensagemEnviada = mensagemEnviada;

            return output;
        }
        catch (Exception)
        {
            output.Sucesso = false;
            return output;
        }
    }
}
