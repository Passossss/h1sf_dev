using System;

namespace H1SF.Application.Services.PreparaDetalhePtd
{
    /// <summary>
    /// Tradução isolada do trecho COBOL 755-00-PREPARA-DETALHE-PTD para C#.
    /// Arquivo independente, sem misturar trechos anteriores.
    /// </summary>
    public class PreparaDetalhePtd755Service : IPreparaDetalhePtd755Service
    {
        // Campos que refletem os nomes COBOL (WS / CB...)
        public int WS01NumViasPtd { get; set; }
        public string WS36CdMercDst { get; set; } = string.Empty; // normalmente 1-char
        public string WS36FaseFtrm { get; set; } = string.Empty;  // normalmente 1-char

        public string WS01CdSqnPjlPtd { get; set; } = string.Empty;
        public int WS01CdSqnPjlPtdN { get; set; }

        public string CB0002DreCdSqnPjl { get; set; } = string.Empty;

        public PreparaDetalhePtd755Service() { }

        /// <summary>
        /// Executa a lógica do trecho COBOL 755-00.
        /// </summary>
        public void PreparaDetalhePtd()
        {
            if (WS01NumViasPtd == 1)
            {
                // MOVE WS01-CD-SQN-PJL-PTD TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = WS01CdSqnPjlPtd;
            }
            else if (WS01NumViasPtd == 3 &&
                     string.Equals(WS36CdMercDst, "D", StringComparison.Ordinal) &&
                     string.Equals(WS36FaseFtrm, "2", StringComparison.Ordinal))
            {
                // MOVE '96' TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = "96";
            }
            else
            {
                // ADD 1 TO WS01-CD-SQN-PJL-PTD-N
                WS01CdSqnPjlPtdN++;

                // MOVE WS01-CD-SQN-PJL-PTD TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = WS01CdSqnPjlPtd;

                // SUBTRACT 1 FROM WS01-CD-SQN-PJL-PTD-N
                WS01CdSqnPjlPtdN--;
            }
        }
    }
}