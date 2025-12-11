using H1SF.Application.DTOs.ContabilizaItem;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.ContabilizaItem;

/// <summary>
/// Implementação para 845-00-CONTABILIZA-ITEM
/// Contabilização de item do DANFE via MQ
/// </summary>
public class ContabilizaItemService : IContabilizaItem
{
    private readonly ApplicationDbContext _context;

    public ContabilizaItemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContabilizaItemOutput> ExecutarAsync(ContabilizaItemInput input)
    {
        var output = new ContabilizaItemOutput();

        try
        {
            //mock PERFORM 532-00-RECUPERA-ITEM-RCBM
            bool itemRecuperado = true;

            //mock MOVE '01' TO ACB5022-CD-ACES
            string codigoAcesso = "01";

            //mock MOVE ZEROS TO ACB5022-CD-RETR-ECI, ACB5022-CD-RETR-ACES
            string codigoRetornoEci = "0";
            string codigoRetornoAcesso = "0";

            //mock MOVE '03' TO ACB50221-CD-ACES-MQ
            string codigoAcessoMq = "03";

            //mock MOVE 00013 TO ACB50221-ID-FILA-MQ
            int idFilaMq = 13;

            //mock MOVE SPACES TO RRBMQ008-ITEM-NF
            string itemNf = "";

            //mock MOVE SF0002-ITD-DTC-SEL-FTRM TO WS35-ID-CORR-ID-LIT-SC
            //mock MOVE CC0001-NUMERO-CTB-5 TO WS35-ID-CORR-ID-ALF-SC
            string idCorrelacao = input.DataSelecaoFaturamento.ToString("yyyyMMddHHmmss") + input.NumeroNotaContabil;

            //mock INSPECT WS35-ID-CORREL-ID CONVERTING SPACES TO ZEROS
            idCorrelacao = idCorrelacao.Replace(" ", "0");

            //mock MOVE CC0001-DH-EMISSAO-I-SEC TO WS01-CD-RR-CTB-SEC
            //mock MOVE 'D6' TO WS01-CD-RR-CTB-LIT
            //mock MOVE CC0001-NUMERO-CTB TO WS01-CD-RR-CTB-NF
            string codigoRr = input.DataEmissaoSegundos.ToString() + "D6" + input.NumeroNotaContabil;

            //mock MOVE 'NFD' TO RRBMQ008-CD-T-NF
            string tipoNotaFiscal = "NFD";

            //mock MOVE CC0001-NUMERO-CTB-6 TO RRBMQ008-CD-NF
            string codigoNf = input.NumeroNotaContabil.Substring(Math.Max(0, input.NumeroNotaContabil.Length - 6));

            //mock MOVE SR0003-ID-FRN TO RRBMQ008-CD-FRN
            int codigoFornecedor = input.IdFornecedor;

            //mock MOVE CC0004-CPF-CGC TO WS01-CGC-EMIF-ORIG
            //mock conversão de formato CGC
            string cgcCpf = input.CpfCgc;

            //mock MOVE CC0002-IDF-NUM TO RRBMQ008-CD-SQN-NF
            string sequenciaNf = input.IdentificacaoNumero;

            //mock MOVE SF0002-ITD-ID-PECA-LOG TO RRBMQ008-CD-PECA
            string codigoPeca = input.IdPeca;

            //mock MOVE CC0002-NBM-CODIGO TO RRBMQ008-CD-NCM
            string codigoNcm = input.CodigoNbm;

            //mock MOVE CC0002-ALIQ-IPI-LOG TO RRBMQ008-PERC-IPI
            decimal percentualIpi = input.AliquotaIpi;

            //mock MOVE CC0002-ALIQ-ICMS-LOG TO RRBMQ008-PERC-ICMS
            decimal percentualIcms = input.AliquotaIcms;

            //mock MOVE ZEROS TO RRBMQ008-PERC-TRBT-ICMS
            decimal percentualTributoIcms = 0;

            //mock MOVE '1' TO RRBMQ008-CD-FNT
            string codigoFonte = "1";

            //mock MOVE WQ01-NOP-CODIGO-AUX TO RRBMQ008-CD-T-OPRC, RRBMQ008-CD-NOP
            string codigoTipoOperacao = input.CodigoNop;
            string codigoNop = input.CodigoNop;

            //mock MOVE '2' TO RRBMQ008-CD-OPRC
            string codigoOperacao = "2";

            //mock MOVE CC0002-CFOP-CODIGO TO RRBMQ008-CD-CFOP
            string codigoCfop = input.CodigoCfop;

            //mock MOVE CT0001-CD-AUX-ICM TO RRBMQ008-CD-AUX(1:1)
            //mock MOVE CT0001-CD-AUX-IPI TO RRBMQ008-CD-AUX(2:1)
            string codigoAuxiliar = input.CodigoAuxiliarIcm + input.CodigoAuxiliarIpi;

            //mock MOVE 'SF' TO RRBMQ008-CD-RQS, RRBMQ008-CD-CPRR
            string codigoRequisicao = "SF";
            string codigoComprador = "SF";

            //mock MOVE 'UM' TO RRBMQ008-CD-UND-MDA
            string unidadeMedida = "UM";

            //mock MOVE SF0002-ITD-Q-PECA-FTRD-CTB TO RRBMQ008-QT-FTRD, QT-FTRD-CVT, QT-RCBD
            int quantidadeFaturada = input.QuantidadePecaFaturada;

            //mock MOVE WS01-PESO-BRT-EMIF-NUM-R TO WS01-PESO-CTB-PESO
            //mock MOVE WS01-PESO-CTB TO RRBMQ008-VL-PESO-LQD, VL-PESO-BRT
            decimal pesoLiquido = input.PesoBruto;
            decimal pesoBruto = input.PesoBruto;

            //mock MOVE CC0002-PRECO-UNITARIO-CTB TO RRBMQ008-V-PREC-UNT
            decimal valorPrecoUnitario = input.PrecoUnitario;

            //mock MOVE CC0002-PRECO-TOTAL-CTB-1 TO RRBMQ008-V-PREC-TTL
            decimal valorPrecoTotal = input.PrecoTotal1;

            //mock MOVE CC0002-VL-BASE-IPI TO RRBMQ008-V-BS-IPI
            decimal valorBaseIpi = input.ValorBaseIpi;

            //mock MOVE CC0002-VL-BASE-ICMS TO RRBMQ008-V-BS-ICMS
            decimal valorBaseIcms = input.ValorBaseIcms;

            //mock MOVE CC0002-VL-TRIBUTAVEL-STF TO RRBMQ008-V-BS-ICMS-SBSC
            decimal valorBaseIcmsSubstituicao = input.ValorTributavelStf;

            //mock MOVE CC0002-VL-IPI-CTB TO RRBMQ008-V-IPI
            decimal valorIpi = input.ValorIpi;

            //mock MOVE CC0002-VL-ICMS-CTB TO RRBMQ008-V-ICMS
            decimal valorIcms = input.ValorIcms;

            //mock MOVE CC0002-VL-STF-CTB TO RRBMQ008-V-ICMS-SBSC
            decimal valorIcmsSubstituicao = input.ValorStf;

            //mock MOVE CC0002-PRECO-TOTAL-CTB-2 TO RRBMQ008-V-TTL-PRD
            decimal valorTotalProduto = input.PrecoTotal2;

            //mock MOVE CC0002-NBM-CODIGO TO RRBMQ008-CD-NCM-PDD
            string codigoNcmPdd = input.CodigoNbm;

            //mock MOVE CC0002-ALIQ-IPI-CTB TO RRBMQ008-PERC-IPI-PDD
            decimal percentualIpiPdd = input.AliquotaIpiCtb;

            //mock MOVE CC0002-PRECO-UNITARIO-CTB TO RRBMQ008-V-PREC-UNT-PDD
            decimal valorPrecoUnitarioPdd = input.PrecoUnitario;

            //SQL busca nome da peça
            string sqlPeca = @"
                SELECT NM_PECA_PRTG
                FROM TB_SR_PECA_V2
                WHERE ID_PECA = @IdPeca";

            string nomePeca = "";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlPeca;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdPeca", input.IdPeca));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        nomePeca = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //mock MOVE WQ01-NM-PECA-PRTG TO RRBMQ008-TXT-DESC-MAT
            string descricaoMaterial = nomePeca;

            //mock MOVE CT0001-DEB-CD-CT TO RRBMQ008-CD-CT
            string codigoConta = input.CodigoContaDebito;

            //mock MOVE CT0001-DEB-CD-SUB1 TO RRBMQ008-CD-SUB1
            string codigoSub1 = input.CodigoSub1Debito;

            //mock MOVE CT0001-DEB-CD-SUB2 TO RRBMQ008-CD-SUB2
            string codigoSub2 = input.CodigoSub2Debito;

            //mock MOVE SR0003-ID-NF-CTB TO RRBMQ008-CD-DCT-NF-ORIG
            string documentoNfOrigem = input.IdNotaFiscalContabil;

            //mock MOVE CC0002-IDF-NUM-CTB TO RRBMQ008-CD-SQN-NF-ORIG
            string sequenciaNfOrigem = input.IdentificacaoNumeroCtb;

            //mock INSPECT RRBMQ008-CD-SQN-NF-ORIG CONVERTING SPACES TO ZEROS
            sequenciaNfOrigem = sequenciaNfOrigem.Replace(" ", "0");

            //mock MOVE ST0005-CD-GR-PECA TO RRBMQ008-CD-GR-PECA
            string codigoGrupoPeca = input.CodigoGrupoPeca;

            //mock ADD 1 TO WS01-CONT-ITEM-CTB
            int contadorItens = 1;

            //mock MOVE 932 TO ACB50221-TAM-MSG (tamanho mensagem RRBMQ008)
            int tamanhoMensagem = 932;

            //mock PERFORM 555-00-GRAVA-MENSAGEM-MQ
            bool mensagemEnviada = true;

            output.Sucesso = true;
            output.CodigoAcesso = codigoAcesso;
            output.CodigoRetornoEci = codigoRetornoEci;
            output.CodigoRetornoAcesso = codigoRetornoAcesso;
            output.IdMensagemCorrelacao = idCorrelacao;
            output.CodigoRr = codigoRr;
            output.ContadorItensContabilizados = contadorItens;
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
