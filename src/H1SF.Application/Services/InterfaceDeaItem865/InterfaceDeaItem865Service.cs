using H1SF.Application.DTOs.Interface;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.InterfaceDeaItem865;

/// <summary>
/// Implementação para 865-00-INTERFACE-DEA-ITEM
/// Envia dados do item para o sistema DEA com lógica de peças remanufaturadas
/// </summary>
public class InterfaceDeaItem865Service : IInterfaceDeaItem865
{
    private readonly ApplicationDbContext _context;

    public InterfaceDeaItem865Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterfaceDeaItem865Output> ExecutarAsync(InterfaceDeaItem865Input input)
    {
        var output = new InterfaceDeaItem865Output();

        try
        {
            //mock MOVE '00-02' TO SE9-CD-EMP
            string nomeEmpresa = "00-02";

            //mock MOVE '4' TO SE9-ID-TP-INTERFACE
            string idTipoInterface = "4";

            //mock MOVE WS36-CD-SIS-ORIG-DEA TO SE9-ID-SIS-ORI
            string idSistemaOrigem = "SF00";

            //mock MOVE WQ01-SYSDATE-DEA TO SE9-DT-GERA
            string dataGeracao = input.DataSistema;

            //mock IF SF0005-SFT-CD-T-REC = WS36-CD-T-REC-EXPO
            string indicadorItemDea = "5";
            if (input.CodigoTipoRecolhimento == "E")
            {
                indicadorItemDea = "D";
            }

            //mock IF WS36-CD-MERC-DST = 'ZZ'
            if (input.CodigoMercadoriaDestino == "ZZ")
            {
                if (input.IndicadorFaturamentoTrigado == "N")
                {
                    indicadorItemDea = "I";
                }
            }

            //mock MOVE WS01-ID-NUM-CNT TO SE9-ID-NUM-CNT
            string numeroContador = input.IdNumeroContador;

            //mock MOVE WS01-ID-NUM-T-REG TO SE9-ID-NUM-T-REG
            string numeroTotalRegistros = input.IdNumeroTotalRegistros;

            //mock MOVE indicadorItemDea TO SE9-ID-ITEM-DEA
            output.IndicadorItemDea = indicadorItemDea;

            //SQL verifica se peça é remanufaturada
            string sqlVerificaReman = @"
                SELECT CD_ANLS_PECA, IC_RMAN_BK
                FROM TB_SF_PECA_V2
                WHERE ID_PECA_LOG = @IdPeca";

            var parametros = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@IdPeca", input.IdPeca)
            };

            string codigoAnalise = "";
            string indicadorReman = "";

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlVerificaReman;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdPeca", input.IdPeca));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        codigoAnalise = reader.GetString(0);
                        indicadorReman = reader.GetString(1);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //mock IF IC_RMAN_BK = 'S' AND CD_ANLS_PECA = '30'
            bool isPecaReman = false;
            if (indicadorReman == "S" && codigoAnalise == "30")
            {
                //mock PERFORM 160-00-VERIFICA-SE-PECA-REMAN
                isPecaReman = true;
            }

            //mock MOVE CC0001-NUMERO-DEA TO SE9-NM-DEA
            string numeroDea = input.NumeroNotaDea;

            //mock MOVE CC0001-SERIE-SUBSERIE-3 TO SE9-CD-SERIE
            string codigoSerie = input.SerieSubserie;

            //mock MOVE CC0002-IDF-NUM-4 TO SE9-ID-NUM-ITEM
            string idNumeroItem = input.IdentificacaoNumero4;

            //mock MOVE ST0005-NM-FNT-ATND TO SE9-NM-PSO
            string nomePso = input.NomeFonteAtendimento;

            //mock INSPECT SE9-NM-PSO CONVERTING '/' TO ' '
            nomePso = nomePso.Replace("/", " ");

            //mock MOVE CC0002-NBM-CODIGO-DEA TO SE9-CD-NBM
            string codigoNbm = input.CodigoNbm;

            //mock MOVE WS01-SIT-TRIB TO SE9-CD-SIT-TRIB
            string situacaoTributaria = input.SituacaoTributaria;

            //mock MOVE CC0002-ENTSAI-UNI-CODIGO TO SE9-CD-UNI
            string codigoUnidade = input.CodigoUnidade;

            //mock MOVE CC0002-QTD-DEA TO SE9-Q-MERC-X
            int quantidadeMercadoria = input.QuantidadeDea;

            //mock MOVE CC0002-PRECO-UNITARIO-DEA TO SE9-V-UNIT-X
            decimal valorUnitario = input.PrecoUnitarioDea;

            //mock MOVE CC0002-PRECO-TOTAL-CTB-1 TO SE9-V-MERC-X
            decimal valorMercadoria = input.PrecoTotalContabil;

            //mock MOVE CC0002-ALIQ-IPI-LOG-5 TO SE9-P-IPI-X
            decimal percentualIpi = input.AliquotaIpi;

            //mock MOVE CC0002-ALIQ-ICMS-LOG-5 TO SE9-P-ICMS-X
            decimal percentualIcms = input.AliquotaIcms;

            //mock MOVE WS01-TXT-TX-SERV TO SE9-V-TX-SERV-X
            string valorTaxaServico = input.TextoTaxaServico;

            //mock MOVE WS01-TXT-PERC-DSCT TO SE9-P-DSCT-X
            string percentualDesconto = input.TextoPercentualDesconto;

            //mock MOVE CC0002-VL-IPI-DEA TO SE9-V-IPI-X
            decimal valorIpi = input.ValorIpiDea;

            //mock MOVE CC0002-VL-BASE-ICMS-DEA TO SE9-V-BASE-ICMS-X
            decimal valorBaseIcms = input.ValorBaseIcmsDea;

            //mock MOVE CC0002-VL-ICMS-DEA TO SE9-V-ICMS-X
            decimal valorIcms = input.ValorIcmsDea;

            //mock MOVE CC0002-FRETE-DEA TO SE9-V-FRT-X
            decimal valorFrete = input.FreteDea;

            //mock MOVE SF0002-ITD-V-TX-SERV TO SE9-V-SERV-X
            decimal valorServico = input.ValorTaxaServico;

            //mock MOVE CC0002-DESCONTO-ITEM-DEA TO SE9-V-DSCT-X
            decimal valorDesconto = input.DescontoItemDea;

            //mock MOVE WS01-PESO-BRT-EMIF-NUM TO SE9-Q-PESO-BRT-X
            decimal pesoBruto = input.PesoBrutoEmif;

            //mock MOVE SF0002-ITD-ID-VOL-LOG TO SE9-ID-VOL
            string idVolume = input.IdVolume;

            //mock MOVE SF0002-ITD-ID-PDD-LOG TO SE9-ID-PDD
            string idPdd = input.IdPdd;

            //mock MOVE ST0005-ID-PSO TO SE9-ID-PSO
            int idPso = input.IdPso;

            //mock MOVE SF0002-ITD-ID-ETIQ-REC-LOG TO SE9-ID-ETIQ-REC
            string idEtiquetaRec = input.IdEtiquetaRecolhimento;

            //mock MOVE SF0002-ITD-ID-ETIQ-ACND-LOG TO SE9-ID-ETIQ-ACND
            string idEtiquetaAcnd = input.IdEtiquetaAcondicionamento;

            //mock MOVE ST0005-CD-PFO TO SE9-CD-PFO
            string codigoPfo = input.CodigoPfo;

            //mock MOVE CC0002-CFOP-CODIGO TO SE9-CD-CFOP
            string codigoCfop = input.CodigoCfop;

            //mock MOVE CC0002-NOP-COD-1 TO SE9-CD-NOP-1
            //mock MOVE CC0002-NOP-COD-2 TO SE9-CD-NOP-2
            string codigoNop = input.CodigoNop;

            //mock MOVE ST0005-ID-SQN-ITEM-PDD TO SE9-ID-SQN-ITEM-PDD
            int idSequenciaItemPdd = input.IdSequenciaItemPdd;

            //mock MOVE ST0005-ID-SQN-ATND-ITEM TO SE9-ID-SQN-ATND-ITEM
            int idSequenciaAtnd = input.IdSequenciaAtendimentoItem;

            //mock MOVE ST0005-ID-SQN-OCR-FNT TO SE9-ID-SQN-OCR-FNT
            int idSequenciaOcr = input.IdSequenciaOcorrenciaFonte;

            //mock MOVE SF0002-ITD-ID-ITM-DE TO SE9-ID-ITM-DE
            int idItemDe = input.IdItemDe;

            //mock PERFORM 620-00-GRAVA-INTERFACE-DEA (grava SE90052 ou SE90252)
            bool interfaceGravada = true;

            output.Sucesso = true;
            output.NomeEmpresa = nomeEmpresa;
            output.IdTipoInterface = idTipoInterface;
            output.IdSistemaOrigem = idSistemaOrigem;
            output.DataGeracao = dataGeracao;
            output.NumeroContador = numeroContador;
            output.NumeroTotalRegistros = numeroTotalRegistros;
            output.InterfaceGravada = interfaceGravada;

            return output;
        }
        catch (Exception)
        {
            output.Sucesso = false;
            return output;
        }
    }
}
