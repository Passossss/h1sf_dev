using H1SF.Application.DTOs.EmiteLinkH1sf5053;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Application.Services.EmiteLinkH1sf5053;

/// <summary>
/// Implementação para 583-00-EMITE-LINK-H1SF5053
/// Executa link CICS para programa H1SF5053 (interface SYNITF_IMP_EXP)
/// </summary>
public class EmiteLinkH1sf5053Service : IEmiteLinkH1sf5053
{
    private readonly ApplicationDbContext _context;

    public EmiteLinkH1sf5053Service(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmiteLinkH1sf5053Output> ExecutarAsync(EmiteLinkH1sf5053Input input)
    {
        var output = new EmiteLinkH1sf5053Output();

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

            //mock IF SF0018-FTE-CD-T-DE EQUAL 'DUE'
            if (tipoDocEletronico == "DUE")
            {
                output.Sucesso = true;
                output.TipoDocumentoEletronico = tipoDocEletronico;
                output.ProgramaExecutado = false;
                return output;
            }

            //mock MOVE '01' TO WX02-CD-ACES
            string codigoAcesso = "01";

            //mock MOVE ZEROS TO WX02-CD-RETR-ECI, WX02-CD-RETR-ACES
            string codigoRetornoEci = "0";
            string codigoRetornoAcesso = "0";

            //mock MOVE SF0002-ITD-FTR-EXP TO WX02-FTR-EXP
            string faturaExp = input.FaturaExportacao;

            //mock INSPECT WX02-FTR-EXP CONVERTING SPACES TO ZEROS
            faturaExp = faturaExp.Replace(" ", "0");

            //mock EXEC CICS LINK PROGRAM ('H1SF5053')
            //mock COMMAREA (WX02-ARE-H1SF-5053) LENGTH (13)
            bool programaExecutado = true;

            //mock IF WX02-CD-RETR-ECI NOT EQUAL ZEROS OR
            //mock (WX02-CD-RETR-ACES NOT EQUAL ZEROS AND WX02-CD-RETR-ACES NOT EQUAL '01')
            //mock erro na gravação da interface SYNITF_IMP_EXP
            //mock PERFORM 900-00-REGISTRA-ERRO-TRNS

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
