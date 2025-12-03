using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.EntradaNfIcRis
{
    public class EntradaRisRequest
    {
        public int Id { get; set; }
        public string CdAces { get; set; } = "02";                 // WX04-CD-ACES
        public int CdRetrEci { get; set; }                        // WX04-CD-RETR-ECI
        public int CdRetrAces { get; set; }                       // WX04-CD-RETR-ACES
        public string IcTAcao { get; set; } = "I";                // WX04-IC-T-ACAO
        public int CdMercDst { get; set; }                        // WX04-CD-MERC-DST
        public DateTime DtcSelFtrm { get; set; }                  // WX04-DTC-SEL-FTRM
        public string LgonFunc { get; set; } = string.Empty;      // WX04-LGON-FUNC
        public string IdPtcDsp { get; set; } = string.Empty;      // WX04-ID-PTC-DSP
        public string IdImprFtrm { get; set; } = string.Empty;    // WX04-ID-IMPR-FTRM
        public string LgonFuncRsp { get; set; } = string.Empty;   // WX04-LGON-FUNC-RSP
        public string AreParm { get; set; } = string.Empty;       // WQ02-ARE-PARM
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public string? MensagemErro { get; set; }
        public bool Sucesso { get; set; }
    }
}
