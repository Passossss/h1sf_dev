using H1SF.Application.DTOs.DefinidorPjl;

namespace H1SF.Application.Services;

/// <summary>
/// 145-00-DEFINE-PJL-OUTBIN-P
/// Autor: E. FRIOLI JR.
/// </summary>
public class DefinidorPjlOutbinP145Service : IDefinidorPjlOutbinP145
{
    public async Task<DefinidorPjlOutbinP145Output> ExecutarAsync(DefinidorPjlOutbinP145Input input)
    {
        var resultado = new DefinidorPjlOutbinP145Output
        {
            Sucesso = false,
            ComandosPjlGerados = 0
        };

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '145-00-DEFINE-PJL-OUTBIN-P' TO WS35-AUX-TS
        var auxTs = "145-00-DEFINE-PJL-OUTBIN-P";

        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'PACKLIST' TO CB0002-DRE-ID-PRCP-PTD-LIT
        resultado.LiteralPrincipal = "PACKLIST";

        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 170-00-DEFINE-VIAS-ORDENACAO
        await DefinirViasOrdenacaoAsync();

        // 145-10-DEFINE-PJL-OUTBIN2-V1
        if (input.QuantidadeViasPackList <= 0)
        {
            // 145-90-FINALIZA-PARMS
            resultado.LiteralPrincipal = string.Empty;
            resultado.Sucesso = true;
            return resultado;
        }

        if (input.CodigoMercadoriaDestino == "E" && input.CodigoModalidadeTransporte == "6")
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 99 TO WS01-ID-VIA-AUX
            resultado.IdViaAuxiliar = 99;
            resultado.Sucesso = true;
            return resultado;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 1 TO WS01-ID-VIA-AUX
            resultado.IdViaAuxiliar = 1;
        }

        var codigoSequenciaPjl = input.CodigoSequenciaPjlPckLst;

        // Comando 1: FINISH=STAPLE
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 'E%-12345X@PJL SET ' TO WS01-PJL-SET-AUX
        // Este trecho está mockado. Nome COBOL: MOVE '            FINISH=STAPLE' TO WS01-PJL-CMD-AUX
        // Este trecho está mockado. Nome COBOL: MOVE SPACES TO CB0002-DRE-CN-LNH-REL
        // Este trecho está mockado. Nome COBOL: MOVE WS01-PJL-AUX TO CB0002-DRE-CN-LNH-REL
        // Este trecho está mockado. Nome COBOL: MOVE WS01-CD-SQN-PJL-PKLST TO CB0001-RRE-CD-SQN-PJL
        // Este trecho está mockado. Nome COBOL: MOVE WS01-CD-SQN-PJL-PKLST TO CB0002-DRE-CD-SQN-PJL
        // Este trecho está mockado. Nome COBOL: MOVE 'S' TO WS31-CHV-COMANDO-PJL
        // Este trecho está mockado. Nome COBOL: MOVE 'N' TO WS31-CHV-CARREGA-TAB
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await GravarComandoPjlAsync("E%-12345X@PJL SET             FINISH=STAPLE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        // Comando 2: STAPLEOPTION=ONE
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '@PJL SET            ' TO WS01-PJL-SET-AUX
        // Este trecho está mockado. Nome COBOL: MOVE '         STAPLEOPTION=ONE' TO WS01-PJL-CMD-AUX
        await GravarComandoPjlAsync("@PJL SET                     STAPLEOPTION=ONE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        // Comando 3: ORIENTATION=LANDSCAPE
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '@PJL SET            ' TO WS01-PJL-SET-AUX
        // Este trecho está mockado. Nome COBOL: MOVE '    ORIENTATION=LANDSCAPE' TO WS01-PJL-CMD-AUX
        await GravarComandoPjlAsync("@PJL SET                ORIENTATION=LANDSCAPE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        // Comando 4: OUTBIN=OPTIONALOUTBIN2
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE '@PJL SET            ' TO WS01-PJL-SET-AUX
        // Este trecho está mockado. Nome COBOL: MOVE '   OUTBIN=OPTIONALOUTBIN2' TO WS01-PJL-CMD-AUX
        await GravarComandoPjlAsync("@PJL SET               OUTBIN=OPTIONALOUTBIN2", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        // 145-20-DEFINE-PJL-OUTBIN2-V2
        if (input.QuantidadeViasPackList <= 1 ||
            (input.CodigoMercadoriaDestino == "E" &&
             (input.CodigoModalidadeTransporte == "4" || input.CodigoModalidadeTransporte == "5")))
        {
            // 145-90-FINALIZA-PARMS
            resultado.LiteralPrincipal = string.Empty;
            resultado.Sucesso = true;
            return resultado;
        }

        int indicePjlPckLstAux;
        if (input.CodigoMercadoriaDestino == "E" &&
            (input.CodigoModalidadeTransporte == "1" ||
             input.CodigoModalidadeTransporte == "2" ||
             input.CodigoModalidadeTransporte == "3"))
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 20 TO WS01-IND-PJL-PKLST-AUX
            indicePjlPckLstAux = 20;
        }
        else
        {
            //mock
            // Este trecho está mockado. Nome COBOL: MOVE 2 TO WS01-IND-PJL-PKLST-AUX
            indicePjlPckLstAux = 2;
        }

        //mock
        // Este trecho está mockado. Nome COBOL: ADD WS01-IND-PJL-PKLST-AUX TO WS01-CD-SQN-PJL-PKLST-N
        codigoSequenciaPjl += indicePjlPckLstAux;

        // Via 2 - Comandos PJL
        //mock
        // Este trecho está mockado. Nome COBOL: MOVE 2 TO WS01-ID-VIA-AUX
        resultado.IdViaAuxiliar = 2;

        await GravarComandoPjlAsync("E%-12345X@PJL SET             FINISH=STAPLE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        await GravarComandoPjlAsync("@PJL SET                     STAPLEOPTION=ONE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        await GravarComandoPjlAsync("@PJL SET                ORIENTATION=LANDSCAPE", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        await GravarComandoPjlAsync("@PJL SET               OUTBIN=OPTIONALOUTBIN2", codigoSequenciaPjl);
        resultado.ComandosPjlGerados++;

        //mock
        // Este trecho está mockado. Nome COBOL: SUBTRACT WS01-IND-PJL-PKLST-AUX FROM WS01-CD-SQN-PJL-PKLST-N
        codigoSequenciaPjl -= indicePjlPckLstAux;

        // 145-90-FINALIZA-PARMS
        resultado.LiteralPrincipal = string.Empty;
        resultado.Sucesso = true;
        return resultado;
    }

    private async Task DefinirViasOrdenacaoAsync()
    {
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 170-00-DEFINE-VIAS-ORDENACAO
        await Task.CompletedTask;
    }

    private async Task GravarComandoPjlAsync(string comando, int codigoSequencia)
    {
        //mock
        // Este trecho está mockado. Nome COBOL: PERFORM 545-00-GRAVA-DETALHE-REL
        await Task.CompletedTask;
    }
}
