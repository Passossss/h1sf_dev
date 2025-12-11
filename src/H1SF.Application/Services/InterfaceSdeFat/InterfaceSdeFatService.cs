using H1SF.Application.DTOs.InterfaceSdeFat;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.InterfaceSdeFat;

/// <summary>
/// Implementação para 885-00-INTERFACE-S-DE-FAT
/// Interface S-DE para fatura (S57/S58/S7X)
/// </summary>
public class InterfaceSdeFatService : IInterfaceSdeFat
{
    private readonly ApplicationDbContext _context;

    public InterfaceSdeFatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterfaceSdeFatOutput> ExecutarAsync(InterfaceSdeFatInput input)
    {
        var output = new InterfaceSdeFatOutput();

        try
        {
            //mock MOVE 'CBL' TO SE0001-IDE-NM-EMP
            string nomeEmpresa = "CBL";

            string idTipoInterface = "";
            string idSistemaOrigem = "";

            //mock IF ST0001-CD-REGR-FTRM EQUAL 'I'
            if (input.CodigoRegraFaturamento == "I")
            {
                idTipoInterface = "FATURA_S57";
                idSistemaOrigem = "BPISPRA";
            }
            //mock IF ST0001-CD-REGR-FTRM EQUAL 'N'
            else if (input.CodigoRegraFaturamento == "N")
            {
                idTipoInterface = "FATURA_S7X";
                idSistemaOrigem = "BPISREB";
            }
            else
            {
                idTipoInterface = "FATURA_S58";
                idSistemaOrigem = "BPIS";
            }

            //mock MOVE WQ01-SYSDATE-S TO SE0001-IDE-DTC-GRC, SE0001-IDE-DTC-ITF
            string dataGeracao = input.DataSistema;

            //mock MOVE WS01-ID-NUM-CNT-S TO SE0001-IDE-NUM-CNT
            string numeroContador = input.IdNumeroContador;

            //mock MOVE ZEROS TO SE0001-IDE-NUM-T-REG, SE0001-IDE-NUM-SQN
            //mock MOVE '*' TO SE0001-IDE-IC-PRC

            //SQL converte data seleção faturamento
            string sqlData = @"
                SELECT TO_CHAR(TO_DATE(@DataSel, 'YYYYMMDDHH24MISS'), 'DD-MM-YYYY')
                FROM DUAL
                WHERE ROWNUM = 1";

            string dataInlFtrm = "";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlData;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@DataSel", 
                    input.DataSelecaoFaturamento.ToString("yyyyMMddHHmmss")));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dataInlFtrm = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //mock MOVE SF0002-ITD-FTR-EXP TO SE90041-ID-FTR-S57
            string idFaturaS57 = input.FaturaExportacao;

            //mock INSPECT SE90041-ID-FTR-S57 CONVERTING SPACES TO ZEROS
            idFaturaS57 = idFaturaS57.Replace(" ", "0");

            //mock MOVE SF0001-PTD-ID-CLI TO SE90041-ID-CLI
            int idCliente = input.IdCliente;

            //mock MOVE WQ01-DT-INL-FTRM TO SE90041-DTC-EMS
            string dataEmissao = dataInlFtrm;

            //mock MOVE WS01-Q-TTL-ITEM-FAT TO SE90041-QT-ITEM
            int quantidadeItem = input.QuantidadeTotalItemFaturado;

            //mock normaliza código tipo recolhimento
            string codigoTipoRecProcessado = input.CodigoTipoRecolhimento;

            if (input.CodigoTipoRecolhimento == "R1" || input.CodigoTipoRecolhimento == "RA" || 
                input.CodigoTipoRecolhimento == "RG" || input.CodigoTipoRecolhimento == "U1" || 
                input.CodigoTipoRecolhimento == "UA" || input.CodigoTipoRecolhimento == "UG")
            {
                codigoTipoRecProcessado = "R1";
            }
            else if (input.CodigoTipoRecolhimento == "R2" || input.CodigoTipoRecolhimento == "RB" || 
                     input.CodigoTipoRecolhimento == "RH" || input.CodigoTipoRecolhimento == "U2" || 
                     input.CodigoTipoRecolhimento == "UB" || input.CodigoTipoRecolhimento == "UH")
            {
                codigoTipoRecProcessado = "R2";
            }
            else if (input.CodigoTipoRecolhimento == "R3" || input.CodigoTipoRecolhimento == "RC" || 
                     input.CodigoTipoRecolhimento == "RI" || input.CodigoTipoRecolhimento == "U3" || 
                     input.CodigoTipoRecolhimento == "UC" || input.CodigoTipoRecolhimento == "UI")
            {
                codigoTipoRecProcessado = "R3";
            }
            else if (input.CodigoTipoRecolhimento == "R4" || input.CodigoTipoRecolhimento == "RD" || 
                     input.CodigoTipoRecolhimento == "RK" || input.CodigoTipoRecolhimento == "U4" || 
                     input.CodigoTipoRecolhimento == "UD" || input.CodigoTipoRecolhimento == "UK")
            {
                codigoTipoRecProcessado = "R4";
            }
            else if (input.CodigoTipoRecolhimento == "R5" || input.CodigoTipoRecolhimento == "RE" || 
                     input.CodigoTipoRecolhimento == "RL" || input.CodigoTipoRecolhimento == "U5" || 
                     input.CodigoTipoRecolhimento == "UE" || input.CodigoTipoRecolhimento == "UL")
            {
                codigoTipoRecProcessado = "R5";
            }
            else if (input.CodigoTipoRecolhimento == "R6" || input.CodigoTipoRecolhimento == "RF" || 
                     input.CodigoTipoRecolhimento == "RN" || input.CodigoTipoRecolhimento == "U6" || 
                     input.CodigoTipoRecolhimento == "UF" || input.CodigoTipoRecolhimento == "UN")
            {
                codigoTipoRecProcessado = "R6";
            }

            //mock MOVE SF0002-ITD-CD-MOD-TRSP-LOG TO SE90041-CD-MOD-TRSP
            string codigoModalidadeTransporte = input.CodigoModalidadeTransporte;

            //mock MOVE SF0002-ITD-ID-PDD-LOG TO SE90041-ID-PDD
            string idPdd = input.IdPdd;

            //mock IF ST0001-CD-REGR-FTRM EQUAL 'N'
            string idFaturaApi = "";
            if (input.CodigoRegraFaturamento == "N")
            {
                idFaturaApi = input.IdFaturaApi;
            }

            //mock PERFORM 620-00-GRAVA-INTERFACE-DEA
            bool interfaceGravada = true;

            //mock 885-30-FINALIZA-INTERFACE
            //mock grava registro de finalização (999999999, 'N')
            //mock PERFORM 620-00-GRAVA-INTERFACE-DEA

            output.Sucesso = true;
            output.NomeEmpresa = nomeEmpresa;
            output.IdTipoInterface = idTipoInterface;
            output.IdSistemaOrigem = idSistemaOrigem;
            output.DataGeracao = dataGeracao;
            output.NumeroContador = numeroContador;
            output.CodigoTipoRecolhimentoProcessado = codigoTipoRecProcessado;
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
