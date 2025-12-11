using System;

namespace H1SF.Application.Services.MontaProtocolo
{
    /// <summary>
    /// Interface para o módulo MontaProtocolo (trecho COBOL 710-00...).
    /// Expõe a superfície pública necessária para invocar e preparar a rotina.
    /// </summary>
    public interface IMontaProtocoloService
    {
        // Campos de entrada / configuração
        int WS01NumViasPtd { get; set; }
        string WS36CdMercDst { get; set; }
        string WS36FaseFtrm { get; set; }

        string WS01CdSqnPjlPtd { get; set; }
        int WS01CdSqnPjlPtdN { get; set; }

        // Resultado / campos atualizados pela execução
        string CB0002DreCdSqnPjl { get; set; }

        // Campos usados durante o processamento / visíveis para testes
        int WS01SeqVolPtd { get; set; }
        int WS01SeqVolPtdImp { get; set; }

        string WS01Pcl { get; set; }
        string WS01LinhaImprAux { get; set; }
        string WS01PclDados { get; set; }
        int WS01CalcPosAux { get; set; }

        // Campo usado na impressão de reset
        string WS01PclReset { get; set; }

        /// <summary>
        /// Executa a montagem do protocolo (correspondente a 710-00-MONTA-PROTOCOLO).
        /// </summary>
        void MontaProtocolo();
    }
}