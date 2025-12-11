using System;

namespace H1SF.Application.Services.PreparaDetalhePtd
{
    /// <summary>
    /// Interface para o módulo 755-00-PREPARA-DETALHE-PTD.
    /// Expõe os campos necessários para configuração e execução da rotina.
    /// </summary>
    public interface IPreparaDetalhePtd755Service
    {
        // Entradas / configuração
        int WS01NumViasPtd { get; set; }
        string WS36CdMercDst { get; set; }
        string WS36FaseFtrm { get; set; }

        string WS01CdSqnPjlPtd { get; set; }
        int WS01CdSqnPjlPtdN { get; set; }

        // Saída atualizada pela rotina
        string CB0002DreCdSqnPjl { get; set; }

        /// <summary>
        /// Executa a rotina correspondente ao trecho COBOL 755-00-PREPARA-DETALHE-PTD.
        /// </summary>
        void PreparaDetalhePtd();
    }
}