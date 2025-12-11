using System;
namespace H1SF.Application.Services
{

    public class PclDanfe : IPclDanfe
    {
        /// <summary>
        /// Converted from COBOL: 745-00-PCL-DANFE
        /// Returns true when COBOL would GO TO 745-10-CNTR-PAG-ADICION (WS01-IC-PAG-ADIC == 'S').
        /// </summary>
        public bool Executar(
            ref string ws35AuxTs,
            ref int ws01IdVia,
            string ws36CdMercDst,
            string ws36FaseFtrm,
            ref string ws01CdSqnPjlNf,
            ref string cb0002DreCdSqnPjl,
            ref int ws01NumPagAux,
            string ws01IcPagAdic,
            Action? perform170DefineViasOrdenacao = null
        )
        {
            // LOG / TRACE field
            ws35AuxTs = "745-00-PCL-DANFE";

            // PERFORM 170-00-DEFINE-VIAS-ORDENACAO.
            // If the caller has already converted 170-00-DEFINE-VIAS-ORDENACAO,
            // it can be passed as the perform170DefineViasOrdenacao delegate.
            perform170DefineViasOrdenacao?.Invoke();

            // IF WS01-ID-VIA  EQUAL  2
            if (ws01IdVia == 2)
            {
                // IF WS36-CD-MERC-DST  EQUAL  'D'  AND WS36-FASE-FTRM  EQUAL  '2'
                if (string.Equals(ws36CdMercDst, "D", StringComparison.Ordinal) &&
                    string.Equals(ws36FaseFtrm, "2", StringComparison.Ordinal))
                {
                    ws01CdSqnPjlNf = "97";
                }
                else
                {
                    ws01CdSqnPjlNf = "99";
                }
            }
            else if (ws01IdVia == 3) // ELSE IF WS01-ID-VIA  EQUAL  3
            {
                ws01CdSqnPjlNf = "99";
            }

            // MOVE WS01-CD-SQN-PJL-NF TO CB0002-DRE-CD-SQN-PJL.
            cb0002DreCdSqnPjl = ws01CdSqnPjlNf;

            // ADD 1 TO WS01-NUM-PAG-AUX.
            ws01NumPagAux += 1;

            // IF WS01-IC-PAG-ADIC  EQUAL  'S' GO TO 745-10-CNTR-PAG-ADICION.
            // Return true to signal the caller to continue at the equivalent target.
            return string.Equals(ws01IcPagAdic, "S", StringComparison.Ordinal);
        }
    }
}