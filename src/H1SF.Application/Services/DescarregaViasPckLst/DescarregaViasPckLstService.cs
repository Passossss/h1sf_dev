using H1SF.Application.DTOs.DescarregaVias;

namespace H1SF.Application.Services;

/// <summary>
/// 820-00-DESCARREGA-VIAS-PCK-LST
/// Autor: E. FRIOLI JR.
/// </summary>
public class DescarregaViasPckLstService : IDescarregaViasPckLst
{
    public async Task<DescarregaViasPckLstOutput> ExecutarAsync(DescarregaViasPckLstInput input)
    {
        var resultado = new DescarregaViasPckLstOutput
        {
            Sucesso = false
        };

        // 820-10-ATUALIZA-CAMPOS

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'N' TO WS31-CHV-CARREGA-TAB
        var chaveCarregaTabela = "N";

        //mock
        // Este trecho está mockado. Nome COBOL: ADD 1 TO WS01-NUM-VIAS-VOL
        var numeroViasVolume = input.NumeroViasVolume + 1;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE ZEROS TO WS32-IND-VIAS
        var indiceVias = 0;

        // 820-20-GRAVA-LIN-DET
        // PERFORM 825-00-GRAVA-TAB-PCK-LST UNTIL WS32-IND-VIAS EQUAL WS32-IND-IMPR

        while (indiceVias < input.IndiceImpressao)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: PERFORM 825-00-GRAVA-TAB-PCK-LST
            indiceVias = await GravaTabPckLstAsync(
                indiceVias,
                numeroViasVolume,
                input.CodigoMercadoriaDestino,
                input.CodigoModalidadeTransporte,
                input.CodigoSequenciaPjlPckLst,
                input.LinhasImpressao);

            resultado.NumeroViasProcessadas++;
        }

        resultado.Sucesso = true;
        return resultado;
    }

    /// <summary>
    /// 825-00-GRAVA-TAB-PCK-LST
    /// </summary>
    private async Task<int> GravaTabPckLstAsync(
        int indiceVias,
        int numeroViasVolume,
        string codigoMercadoriaDestino,
        string codigoModalidadeTransporte,
        int codigoSequenciaPjlPckLst,
        string[] linhasImpressao)
    {
        // 825-10-SOMA-IND

        //mock
        // Este trecho está mockado. Nome COBOL: ADD 1 TO WS32-IND-VIAS
        indiceVias++;

        // 825-40-GRAVA-LIN-DET

        var indicePjlPckLstAux = 0;

        // IF WS01-NUM-VIAS-VOL EQUAL 1
        if (numeroViasVolume == 1)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 1 TO WS01-IND-PJL-PKLST-AUX
            indicePjlPckLstAux = 1;
        }
        // IF WS01-NUM-VIAS-VOL EQUAL 2
        else if (numeroViasVolume == 2)
        {
            // IF WS36-CD-MERC-DST EQUAL 'E' AND (SF0002-ITD-CD-MOD-TRSP EQUAL '1' OR '2' OR '3')
            if (codigoMercadoriaDestino == "E" &&
                (codigoModalidadeTransporte == "1" ||
                 codigoModalidadeTransporte == "2" ||
                 codigoModalidadeTransporte == "3"))
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 21 TO WS01-IND-PJL-PKLST-AUX
                indicePjlPckLstAux = 21;
            }
            else
            {
                //mock
                // Este trecho está mockado. Nome COBOL: MOVE 3 TO WS01-IND-PJL-PKLST-AUX
                indicePjlPckLstAux = 3;
            }
        }
        // IF WS01-NUM-VIAS-VOL EQUAL 3
        else if (numeroViasVolume == 3)
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 5 TO WS01-IND-PJL-PKLST-AUX
            indicePjlPckLstAux = 5;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 7 TO WS01-IND-PJL-PKLST-AUX
            indicePjlPckLstAux = 7;
        }

        //mock
        // Este trecho está mockado. Nome COBOL: ADD WS01-IND-PJL-PKLST-AUX TO WS01-CD-SQN-PJL-PKLST-N
        var codigoSequenciaAtualizado = codigoSequenciaPjlPckLst + indicePjlPckLstAux;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-CD-SQN-PJL-PKLST TO CB0002-DRE-CD-SQN-PJL
        var dreCodigoSequenciaPjl = codigoSequenciaAtualizado;

        //mock
        // Este trecho está mockado. Nome COBOL: SUBTRACT WS01-IND-PJL-PKLST-AUX FROM WS01-CD-SQN-PJL-PKLST-N
        codigoSequenciaAtualizado -= indicePjlPckLstAux;

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE WS01-LIN-IMPR(WS32-IND-VIAS) TO WS01-LINHA-IMPR-AUX
        var linhaImpressaoAux = indiceVias < linhasImpressao.Length
            ? linhasImpressao[indiceVias]
            : string.Empty;

        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        // Chamada para gravar detalhe do relatório

        await Task.CompletedTask;
        return indiceVias;
    }
}
