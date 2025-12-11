using System;

namespace H1SF.Application.Services
{
    public class DescarregaViasPtdService: IDescarregaViasPtdService
    {
        // Campos que refletem os nomes COBOL (WS / CB...)
        public string WS35AuxTs { get; set; } = string.Empty;
        public int WS01NumViasPtd { get; set; }
        public string WS36CdMercDst { get; set; } = string.Empty; // normalmente 1-char, mantido como string
        public string WS36FaseFtrm { get; set; } = string.Empty;  // normalmente 1-char, mantido como string

        public string CB0002DreCdSqnPjl { get; set; } = string.Empty;

        public int WS01CdSqnPjlPtdN { get; set; }    // campo usado para ADD / SUBTRACT
        public string WS01CdSqnPjlPtd { get; set; } = string.Empty;

        public string WS31ChvCarregaTab { get; set; } = string.Empty;

        public int WS32IndVias { get; set; }
        public int WS32IndImpr { get; set; }

        // Construtor opcional para inicialização
        public DescarregaViasPtdService()
        {
        }

        // Executa a seção principal 810-00...
        public void Execute()
        {
            DescarregaViasPtd();
        }

        // 810-00-DESCARREGA-VIAS-PTD
        private void DescarregaViasPtd()
        {
            WS35AuxTs = "810-00-DESCARREGA-VIAS-PTD";

            if (WS01NumViasPtd == 3 &&
                WS36CdMercDst == "D" &&
                WS36FaseFtrm == "2")
            {
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

            AtualizaCampos();

            GravaLinDet();

            // 810-99-DESCARREGA-EXIT -> retorno implícito
        }

        // 810-10-ATUALIZA-CAMPOS
        private void AtualizaCampos()
        {
            WS31ChvCarregaTab = "N";
            WS01NumViasPtd += 1;
            WS32IndVias = 0; // MOVE ZEROS TO WS32-IND-VIAS
        }

        // 810-20-GRAVA-LIN-DET
        private void GravaLinDet()
        {
            // PERFORM 815-00-GRAVA-TAB-PTD UNTIL WS32-IND-VIAS EQUAL WS32-IND-IMPR
            // Implementação: chama repetidamente até a condição ser verdadeira.
            // O método GravaTabPtd() deve atualizar WS32IndVias para progredir.
            while (WS32IndVias != WS32IndImpr)
            {
                GravaTabPtd();
            }
        }

        // 815-00-GRAVA-TAB-PTD (stub)
        // Substitua/espere que essa rotina atualize WS32IndVias (por exemplo, incrementando),
        // para que o loop em GravaLinDet avance e termine.
        protected virtual void GravaTabPtd()
        {
            // Comportamento padrão: incrementa o indicador para evitar loop infinito.
            // Na aplicação real, escreva a lógica de gravação/registro e atualize WS32IndVias conforme necessário.
            WS32IndVias++;
        }
    }
}