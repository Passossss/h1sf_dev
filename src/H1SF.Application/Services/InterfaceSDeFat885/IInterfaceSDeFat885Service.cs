using System;

namespace H1SF.Application.Services.InterfaceSDeFat
{
    /// <summary>
    /// Interface para o módulo 885-00-INTERFACE-S-DE-FAT.
    /// Expõe entradas, saídas relevantes e a operação principal Execute().
    /// </summary>
    public interface IInterfaceSDeFat885Service
    {
        // Entradas / contexto
        string ST0001CdRegrFtrm { get; set; }
        string SF0001PtdDtcSelFtrm { get; set; }
        string SF0002ItdFtrExp { get; set; }
        string SF0001PtdIdCli { get; set; }
        string SF0001PtdCdTRec { get; set; }
        string SF0002ItdCdModTrspLog { get; set; }
        string SF0002ItdIdPddLog { get; set; }
        string SF0002ItdIdFtrApiLog { get; set; }

        int WS01QTtlItemFat { get; set; }

        // Saídas / campos gerados pela rotina
        string SE0001IdeIdTItf { get; set; }
        string SE0001IdeIdStmOrgm { get; set; }
        string SE0001IdeTxtMsg { get; set; }
        long SE0001IdeNumTReg { get; set; }

        string SE90041IdFtrS57 { get; set; }
        string SE90041IdCli { get; set; }
        string SE90041DtcEms { get; set; }
        int SE90041QtItem { get; set; }
        string SE90041CdTRec { get; set; }
        string SE90041CdModTrsp { get; set; }
        string SE90041IdPdd { get; set; }
        string SE90041IdFtrApi { get; set; }
        string SE90041TxtMsg { get; set; }

        string SE90241TxtMsg { get; set; }

        // Campo auxiliar preenchido no processamento
        string WQ01DtInlFtrm { get; set; }

        /// <summary>
        /// Executa a lógica do módulo (corresponde às sections do COBOL 885).
        /// </summary>
        void Execute();
    }
}