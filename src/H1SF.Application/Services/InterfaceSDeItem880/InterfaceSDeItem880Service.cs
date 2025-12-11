using H1SF.Application.DTOs.Interface;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.InterfaceSDeItem880;

/// <summary>
/// Implementação para 880-00-INTERFACE-S-DE-ITEM
/// Envia dados de item para sistema S-DE (S57/S58) com cálculos em USD
/// </summary>
public class InterfaceSDeItem880Service : IInterfaceSDeItem880
{
    private readonly ApplicationDbContext _context;

    public InterfaceSDeItem880Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterfaceSDeItem880Output> ExecutarAsync(InterfaceSDeItem880Input input)
    {
        var output = new InterfaceSDeItem880Output();

        try
        {
            //mock MOVE '00-02' TO SE9-CD-EMP
            string nomeEmpresa = "00-02";

            //mock MOVE '5' TO SE9-ID-TP-INTERFACE
            string idTipoInterface = "5";

            //mock MOVE WS36-CD-SIS-ORIG-SDE TO SE9-ID-SIS-ORI
            string idSistemaOrigem = "SF00";

            //mock MOVE WQ01-SYSDATE-S TO SE9-DT-GERA
            string dataGeracao = input.DataSistema;

            //SQL busca peso líquido
            string sqlPeso = @"
                SELECT SUM(PSO.Q_PESO_LIQ)
                FROM TB_SF_PSO_V2 PSO
                WHERE PSO.CD_MERC_DST = @CodigoMercDst
                  AND PSO.DTC_SEL_FTRM = @DataSelFtrm
                  AND PSO.LGON_FUNC = @LoginFunc
                  AND PSO.ID_PECA_LOG = @IdPeca";

            decimal pesoLiquido = 0;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlPeso;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CodigoMercDst", input.CodigoMercadoriaDestino));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DataSelFtrm", input.DataSelecaoFaturamento));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoginFunc", input.LoginFuncionario));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdPeca", input.IdPeca));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        pesoLiquido = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //SQL busca taxa de câmbio
            string sqlTaxa = @"
                SELECT CC0001.VL_TAXA_CAMBIO
                FROM TB_CC_CTB_V2 CC0001
                INNER JOIN TB_SF_ITD_V2 SF0002 ON SF0002.ID_CTB = CC0001.ID_CTB
                WHERE SF0002.CD_MERC_DST = @CodigoMercDst
                  AND SF0002.DTC_SEL_FTRM = @DataSelFtrm
                  AND SF0002.LGON_FUNC = @LoginFunc
                  AND SF0002.ID_PECA_LOG = @IdPeca
                  AND SF0002.ID_ETIQ_ACND_LOG = @IdEtiqueta";

            decimal taxaCambio = 0;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlTaxa;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CodigoMercDst", input.CodigoMercadoriaDestino));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DataSelFtrm", input.DataSelecaoFaturamento));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoginFunc", input.LoginFuncionario));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdPeca", input.IdPeca));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdEtiqueta", input.IdEtiquetaAcondicionamento));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        taxaCambio = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //SQL busca somatórios
            string sqlSomatorios = @"
                SELECT SUM(SF0002.Q_PECA_FTRD),
                       SUM(CC0002.PRECO_TOT_DEA_US),
                       SUM(CC0002.PRECO_TOT_CTB_US),
                       SUM(SF0002.V_US_RT_FRT),
                       SUM(SF0002.V_US_RT_SGR),
                       SUM(SF0002.V_US_RT_ODA),
                       SUM(SF0002.V_US_ARED_CES_ODA),
                       SUM(SF0002.V_AI_CHRG_API)
                FROM TB_SF_ITD_V2 SF0002
                INNER JOIN TB_CC_ITD_V2 CC0002 ON CC0002.ID_CTB = SF0002.ID_CTB 
                                                AND CC0002.IDF_NUM = SF0002.IDF_NUM
                WHERE SF0002.CD_MERC_DST = @CodigoMercDst1
                  AND SF0002.DTC_SEL_FTRM = @DataSelFtrm1
                  AND SF0002.LGON_FUNC = @LoginFunc1
                  AND SF0002.ID_PECA_LOG = @IdPeca1
                  AND SF0002.CD_MERC_DST = @CodigoMercDst2
                  AND SF0002.DTC_SEL_FTRM = @DataSelFtrm2
                  AND SF0002.LGON_FUNC = @LoginFunc2
                  AND SF0002.FTR_EXP = @FtrExp";

            decimal qtdTotal = 0;
            decimal precoTotalDea = 0;
            decimal precoTotalCtb = 0;
            decimal valorFrete = 0;
            decimal valorSeguro = 0;
            decimal valorOutras = 0;
            decimal valorAred = 0;
            decimal valorAi = 0;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlSomatorios;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CodigoMercDst1", input.CodigoMercadoriaDestino));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DataSelFtrm1", input.DataSelecaoFaturamento));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoginFunc1", input.LoginFuncionario));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IdPeca1", input.IdPeca));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CodigoMercDst2", input.WqCodigoMercadoriaDestino));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DataSelFtrm2", input.WqDataSelecaoFaturamento));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoginFunc2", input.WqLoginFuncionario));
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FtrExp", input.FaturaExportacao));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        qtdTotal = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                        precoTotalDea = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                        precoTotalCtb = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                        valorFrete = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                        valorSeguro = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                        valorOutras = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                        valorAred = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6);
                        valorAi = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //mock MOVE WS01-ID-NUM-CNT-S TO SE9-ID-NUM-CNT
            string numeroContador = input.IdNumeroContador;

            //mock MOVE WS01-ID-NUM-T-REG-S TO SE9-ID-NUM-T-REG
            string numeroTotalRegistros = input.IdNumeroTotalRegistros;

            //mock MOVE CC0001-NUMERO TO SE9-NM-NTA-FSCL
            string numeroNotaFiscal = input.NumeroNota;

            //mock MOVE CC0002-IDF-NUM TO SE9-ID-NUM-ITEM
            string idNumeroItem = input.IdentificacaoNumero;

            //mock MOVE CC0002-NBM-CODIGO-DEA TO SE9-CD-NCM
            string codigoNcm = input.CodigoNbm;

            //mock MOVE CC0002-QTD-DEA TO SE9-Q-ITEM-X
            int quantidadeItem = input.QuantidadeDea;

            //mock IF ST0001-CD-REGR-FTRM = WS36-CD-REGR-FTRM-S57
            string tipoInterface = "S57";
            if (input.CodigoRegraFaturamento == "S58")
            {
                tipoInterface = "S58";
            }
            else if (input.CodigoRegraFaturamento.StartsWith("S7"))
            {
                tipoInterface = "S7X";
            }

            //mock COMPUTE SE9-V-UNIT-MERC-X = CC0002-PRECO-UNIT-DEA-US-X / WQ02-Q-PESO-LIQ
            decimal valorUnitMerc = 0;
            if (pesoLiquido != 0)
            {
                valorUnitMerc = input.PrecoUnitarioDeaUsd / pesoLiquido;
            }

            //mock MOVE CC0002-PRECO-TOT-DEA-US-X TO SE9-V-MERC-X
            decimal valorMercadoria = input.PrecoTotalDeaUsd;

            //mock MOVE SF0002-ITD-ID-VOL-LOG TO SE9-ID-VOL
            string idVolume = input.IdVolume;

            //mock MOVE SF0002-ITD-ID-ETIQ-REC-LOG TO SE9-ID-ETIQ-REC
            string idEtiquetaRec = input.IdEtiquetaRecolhimento;

            //mock MOVE SF0002-ITD-ID-ITM-DE TO SE9-ID-ITM-DE
            int idItemDe = input.IdItemDe;

            //mock MOVE ST0005-CD-PFO TO SE9-CD-PFO
            string codigoPfo = input.CodigoPfo;

            //mock COMPUTE SE9-V-FRT-X = SF0002-ITD-V-US-RT-FRT / WQ02-Q-PESO-LIQ
            decimal valorFreteCalc = 0;
            if (pesoLiquido != 0)
            {
                valorFreteCalc = input.ValorFreteUsd / pesoLiquido;
            }

            //mock COMPUTE SE9-V-SGR-X = SF0002-ITD-V-US-RT-SGR / WQ02-Q-PESO-LIQ
            decimal valorSeguroCalc = 0;
            if (pesoLiquido != 0)
            {
                valorSeguroCalc = input.ValorSeguroUsd / pesoLiquido;
            }

            //mock COMPUTE SE9-V-ODA-X = SF0002-ITD-V-US-RT-ODA / WQ02-Q-PESO-LIQ
            decimal valorOdasCalc = 0;
            if (pesoLiquido != 0)
            {
                valorOdasCalc = input.ValorOutrasDespesasUsd / pesoLiquido;
            }

            //mock COMPUTE SE9-V-ARED-CES-ODA-X = SF0002-ITD-V-US-ARED-CES-ODA / WQ02-Q-PESO-LIQ
            decimal valorAredCalc = 0;
            if (pesoLiquido != 0)
            {
                valorAredCalc = input.ValorAredondamentoCesOda / pesoLiquido;
            }

            //mock COMPUTE SE9-V-AI-CHRG-API-X = SF0002-ITD-V-AI-CHRG-API / WQ02-Q-PESO-LIQ
            decimal valorAiCalc = 0;
            if (pesoLiquido != 0)
            {
                valorAiCalc = input.ValorAiChrgApi / pesoLiquido;
            }

            //mock COMPUTE SE9-Q-PESO-LIQ-X = WQ02-Q-PESO-LIQ / WQ03-Q-ITEM-T
            decimal pesoLiquidoCalc = 0;
            if (qtdTotal != 0)
            {
                pesoLiquidoCalc = pesoLiquido / qtdTotal;
            }

            //mock PERFORM 620-00-GRAVA-INTERFACE-DEA (grava SE90043)
            bool interfaceGravada = true;

            output.Sucesso = true;
            output.NomeEmpresa = nomeEmpresa;
            output.IdTipoInterface = idTipoInterface;
            output.IdSistemaOrigem = idSistemaOrigem;
            output.DataGeracao = dataGeracao;
            output.QuantidadeTotalItemFaturado = qtdTotal;
            output.PesoLiquidoTotal = pesoLiquido;
            output.TaxaCambio = taxaCambio;
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
