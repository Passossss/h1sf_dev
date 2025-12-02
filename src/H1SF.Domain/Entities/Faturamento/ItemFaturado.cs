using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    /// <summary>
    /// Tabela H1SF.ITD_ITMFATURADO
    /// </summary>
    public class ItemFaturado
    {
        public int ItdCdTRec { get; set; }
        public int ItdCdMercDst { get; set; }
        public DateTime ItdDtcSelFtrm { get; set; }
        public string ItdLgonFunc { get; set; } = string.Empty;
        
        /// <summary>
        /// ITD_ID_PTC_DSP - Identificador Protocolo Despacho
        /// </summary>
        public string? ItdIdPtcDsp { get; set; }
        
        // Campos para 535-00-ATUALIZA-PWS
        public int ItdIdCli { get; set; }
        public int ItdIdVol { get; set; }
        public int ItdIdEtiqRec { get; set; }
        public int ItdQPecaFtrd { get; set; }
        public string? ItdIdNfRef { get; set; }
        public int ItdIdMtz { get; set; }
        public string? ItdIdNf { get; set; }
        public string? ItdCdModTrsp { get; set; }
    }
}
