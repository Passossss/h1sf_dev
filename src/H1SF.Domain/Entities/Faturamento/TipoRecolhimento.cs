using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class TipoRecolhimento
    {
        public int CdFbr { get; set; }
        public int CdTAtnd { get; set; }
        public int CdRegrDct { get; set; }
        public int CdRegrFtrm { get; set; }
        public int CdTRec { get; set; }
    }
}
