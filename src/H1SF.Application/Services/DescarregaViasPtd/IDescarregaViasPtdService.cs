using System;

namespace H1SF.Application.Services
{
    public interface IDescarregaViasPtdService
    {
        // Entradas principais
        int WS01NumViasPtd { get; set; }
        string WS36CdMercDst { get; set; }
        string WS36FaseFtrm { get; set; }
        string WS01CdSqnPjlPtd { get; set; }
        int WS32IndImpr { get; set; }

        // Saídas / estado resultante
        string CB0002DreCdSqnPjl { get; }
        int WS01CdSqnPjlPtdN { get; }

        // Execução da rotina principal (equivalente ao PERFORM/SECTION)
        void Execute();
    }
}