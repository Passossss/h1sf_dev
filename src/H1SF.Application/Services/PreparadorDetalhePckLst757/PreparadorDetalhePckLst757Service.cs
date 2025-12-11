using H1SF.Application.DTOs.PreparadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 757-00-PREPARA-DETALHE-PCK-LST
/// Autor: E. FRIOLI JR.
/// </summary>
public class PreparadorDetalhePckLst757Service : IPreparadorDetalhePckLst757
{
    public async Task<PreparadorDetalhePckLst757Output> ExecutarAsync(PreparadorDetalhePckLst757Input input)
    {
        var resultado = new PreparadorDetalhePckLst757Output
        {
            Sucesso = false
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '757-00-PREPARA-DETALHE-PCK-LST' TO WS35-AUX-TS
        var auxTs = "757-00-PREPARA-DETALHE-PCK-LST";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'BPIS' TO CB0002-DRE-CD-STM
        resultado.CodigoSistema = "BPIS";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE CB0001-RRE-DTC-GRC TO CB0002-DRE-DTC-GRC
        resultado.DataGeracao = input.DataGeracao;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WQ01-DOF-IMPOR-NUMERO TO CB0002-DRE-ID-PRCP
        resultado.IdPrincipal = input.NumeroImportacao;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE SF0001-PTD-ID-PTC-DSP TO CB0002-DRE-CD-SQN-DCT
        resultado.CodigoSequenciaDocumento = input.IdProtocoloDespacho;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'PACKLIST' TO CB0002-DRE-ID-PRCP-PTD-LIT
        resultado.LiteralPrincipal = "PACKLIST";

        // Calcula índice PJL baseado no número de vias
        var indicePjlPckLstAux = CalcularIndicePjl(
            input.NumeroViasVolume,
            input.CodigoMercadoriaDestino,
            input.CodigoModalidadeTransporte);

        //mock
        // Este trecho está mockado. Nome COBOL: ADD WS01-IND-PJL-PKLST-AUX TO WS01-CD-SQN-PJL-PKLST-N
        var codigoSequenciaPjl = input.CodigoSequenciaPjlPckLst + indicePjlPckLstAux;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-CD-SQN-PJL-PKLST TO CB0002-DRE-CD-SQN-PJL
        resultado.CodigoSequenciaPjl = codigoSequenciaPjl;

        //mock
        // Este trecho está mockado. Nome COBOL: SUBTRACT WS01-IND-PJL-PKLST-AUX FROM WS01-CD-SQN-PJL-PKLST-N
        codigoSequenciaPjl -= indicePjlPckLstAux;

        resultado.Sucesso = true;
        return await Task.FromResult(resultado);
    }

    private int CalcularIndicePjl(int numeroViasVolume, string codigoMercadoriaDestino, string codigoModalidadeTransporte)
    {
        if (numeroViasVolume == 1)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 1 TO WS01-IND-PJL-PKLST-AUX
            return 1;
        }
        else if (numeroViasVolume == 2)
        {
            if (codigoMercadoriaDestino == "E" &&
                (codigoModalidadeTransporte == "1" ||
                 codigoModalidadeTransporte == "2" ||
                 codigoModalidadeTransporte == "3"))
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 21 TO WS01-IND-PJL-PKLST-AUX
                return 21;
            }
            else
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 3 TO WS01-IND-PJL-PKLST-AUX
                return 3;
            }
        }
        else if (numeroViasVolume == 3)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 5 TO WS01-IND-PJL-PKLST-AUX
            return 5;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 7 TO WS01-IND-PJL-PKLST-AUX
            return 7;
        }
    }
}
