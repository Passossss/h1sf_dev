using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.DreDetalhesRelatorio
{
    public class DetalheRelatorio
    {
        public int Id { get; set; }
        public string DreCdStm { get; set; } = string.Empty;       // DRE_CD_STM
        public DateTime DreDtcGrc { get; set; }                    // DRE_DTC_GRC
        public string DreIdPrcp { get; set; } = string.Empty;     // DRE_ID_PRCP
        public string DreCdSqnDct { get; set; } = string.Empty;   // DRE_CD_SQN_DCT
        public string DreCdSqnPjl { get; set; } = string.Empty;   // DRE_CD_SQN_PJL
        public int DreCdSqnLnh { get; set; }                      // DRE_CD_SQN_LNH
        public string DreCnLnhRel { get; set; } = string.Empty;   // DRE_CN_LNH_REL
        public string DreIdVia { get; set; } = string.Empty;      // DRE_ID_VIA
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
