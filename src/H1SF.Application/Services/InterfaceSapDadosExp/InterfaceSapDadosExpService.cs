using H1SF.Application.DTOs.InterfaceSapDadosExp;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.InterfaceSapDadosExp;

/// <summary>
/// Implementação para 830-00-INTERFACE-SAP-DADOS-EXP
/// Executa interface SAP para contas a receber de exportação
/// </summary>
public class InterfaceSapDadosExpService : IInterfaceSapDadosExp
{
    private readonly ApplicationDbContext _context;

    public InterfaceSapDadosExpService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InterfaceSapDadosExpOutput> ExecutarAsync(InterfaceSapDadosExpInput input)
    {
        var output = new InterfaceSapDadosExpOutput();

        try
        {
            //SQL verifica tipo documento eletrônico
            string sqlTipoDoc = @"
                SELECT FTE_CD_T_DE
                FROM TB_SF_FTE_V2
                WHERE FTE_FTR_EXP = @FaturaExp";

            string tipoDocEletronico = "";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlTipoDoc;
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FaturaExp", input.FaturaExportacao));
                await _context.Database.OpenConnectionAsync();
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        tipoDocEletronico = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    }
                }
                
                await _context.Database.CloseConnectionAsync();
            }

            //mock comentário COBOL: INCLUIR GO TO ABAIXO PARA NAO ENVIAR INTERFACE SAP
            //mock CONTAS A RECEBER SEM A ATUALIZACAO DO DUE
            //mock SICRONIZAR ALTERACAO COM PROGRAMA H1SF0037 ROTINA 140-00-FINALIZA-FASE-FTRM
            //mock IF SF0018-FTE-CD-T-DE EQUAL 'DUE'
            //mock GO TO 830-99-INTERFACE-EXIT (comentado no COBOL original)

            //mock MOVE '10' TO WX03-CD-ACES
            string codigoAcesso = "10";

            //mock MOVE ZEROS TO WX03-CD-RETR-ECI, WX03-CD-RETR-ACES
            string codigoRetornoEci = "0";
            string codigoRetornoAcesso = "0";

            //mock MOVE SF0002-ITD-FTR-EXP TO WX03-FTR-EXP
            string faturaExp = input.FaturaExportacao;

            //mock INSPECT WX03-FTR-EXP CONVERTING SPACES TO ZEROS
            faturaExp = faturaExp.Replace(" ", "0");

            //mock PERFORM 582-00-EMITE-LINK-H1SF5049
            bool programaExecutado = true;

            //mock IF WX03-CD-RETR-ECI NOT EQUAL ZEROS OR
            //mock WX03-CD-RETR-ACES NOT EQUAL ZEROS
            //mock erro na gravação da interface SAP
            //mock PERFORM 900-00-REGISTRA-ERRO-TRNS

            //mock SQL grande comentado no COBOL original que busca dados de:
            //mock - FTE_FATURAS_TRSP_EXP (fatura)
            //mock - SHT_SHIP_TO (ship to)
            //mock - CON_CONSIGNATARIO (consignatário)
            //mock - IMP_IMPORTADOR (importador)
            //mock - SUP_FORNECEDOR (fornecedor)

            //mock monta estrutura ST8001-ST8001D com dados de:
            //mock - código acesso '23'
            //mock - código fábrica 'UN'
            //mock - modalidade transporte
            //mock - fatura exportação
            //mock - documento eletrônico
            //mock - fornecedor SPR
            //mock - dados emitente (SE, E8143P0)
            //mock - dados ship to (ST)
            //mock - dados consignatário (BY)
            //mock - dados importador (IC)
            //mock - termo pagamento
            //mock - data saída fábrica

            //mock MOVE '01' TO ACB5022-CD-ACES
            //mock MOVE '03' TO ACB50221-CD-ACES-MQ
            //mock MOVE 00005 TO ACB50221-ID-FILA-MQ
            //mock MOVE 'RES-DANFE' TO ACB50221-CHV-PRAD
            //mock MOVE 851 TO ACB50221-TAM-MSG
            //mock PERFORM 555-00-GRAVA-MENSAGEM-MQ

            output.Sucesso = true;
            output.CodigoAcesso = codigoAcesso;
            output.CodigoRetornoEci = codigoRetornoEci;
            output.CodigoRetornoAcesso = codigoRetornoAcesso;
            output.TipoDocumentoEletronico = tipoDocEletronico;
            output.ProgramaExecutado = programaExecutado;

            return output;
        }
        catch (Exception)
        {
            output.Sucesso = false;
            return output;
        }
    }
}
