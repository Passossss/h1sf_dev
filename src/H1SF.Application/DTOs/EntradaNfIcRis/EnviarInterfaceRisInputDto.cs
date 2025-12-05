using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTOs.EntradaNfIcRis
{
    public class EnviarInterfaceRisInputDto
    {
        public string CdMercDst { get; set; }                       // WQ02-CD-MERC-DST
        public DateTime DtcSelFtrm { get; set; }                 // WQ02-DTC-SEL-FTRM
        public string LgonFunc { get; set; } = string.Empty;     // WQ02-LGON-FUNC
        public string AreParm { get; set; } = string.Empty;      // WQ02-ARE-PARM (opcional)
    }
}
