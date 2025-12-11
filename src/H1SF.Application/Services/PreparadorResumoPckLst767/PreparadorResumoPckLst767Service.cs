using H1SF.Application.DTOs.PreparadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 767-00-PREPARA-RESUMO-PCK-LST
/// Autor: E. FRIOLI JR.
/// </summary>
public class PreparadorResumoPckLst767Service : IPreparadorResumoPckLst767
{
    public async Task<PreparadorResumoPckLst767Output> ExecutarAsync(PreparadorResumoPckLst767Input input)
    {
        var resultado = new PreparadorResumoPckLst767Output
        {
            Sucesso = false
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '767-00-PREPARA-RESUMO-PCK-LST' TO WS35-AUX-TS
        var auxTs = "767-00-PREPARA-RESUMO-PCK-LST";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'BPIS' TO CB0001-RRE-CD-STM
        resultado.CodigoSistema = "BPIS";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WQ01-DOF-IMPOR-NUMERO TO CB0001-RRE-ID-PRCP
        resultado.IdPrincipal = input.NumeroImportacao;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'PACKLIST' TO CB0001-RRE-ID-PRCP-PTD-LIT
        resultado.LiteralPrincipal = "PACKLIST";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-CD-T-REC TO CB0001-RRE-ID-AUX-IMPS-1
        resultado.IdAuxiliar1 = input.CodigoTipoRecolhimento;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-MTZ TO CB0001-RRE-ID-AUX-IMPS-2
        resultado.IdAuxiliar2 = input.IdMatriz;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-CLI TO CB0001-RRE-ID-AUX-IMPS-3
        resultado.IdAuxiliar3 = input.IdCliente;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-PTC-DSP TO CB0001-RRE-ID-AUX-IMPS-4
        resultado.IdAuxiliar4 = input.IdProtocoloDespacho;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO CB0001-RRE-ID-AUX-IMPS-5
        // Este trecho está mockado. Nome COBOL: MOVE -1 TO WI01-RRE-ID-AUX-IMPS-5
        var idAuxiliar5 = string.Empty;
        var wiIdAuxiliar5 = -1;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'PACKING LIST DE PECAS' TO CB0001-RRE-ID-REL
        resultado.IdRelatorio = "PACKING LIST DE PECAS";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-IMPRESSORA-LASER TO CB0001-RRE-ID-IMPR
        resultado.IdImpressora = input.IdImpressora;

        resultado.Sucesso = true;
        return await Task.FromResult(resultado);
    }
}
