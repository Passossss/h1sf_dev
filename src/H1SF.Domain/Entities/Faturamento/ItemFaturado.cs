using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class ItemFaturado
    {
        public int ItdCdTRec { get; set; }
        public int ItdCdMercDst { get; set; }
        public DateTime ItdDtcSelFtrm { get; set; }
        public string ItdLgonFunc { get; set; } = string.Empty;
    }
}
