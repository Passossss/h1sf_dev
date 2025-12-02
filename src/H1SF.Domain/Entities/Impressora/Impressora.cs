using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities
{
    public class Impressora
    {
        public int IdImpr { get; set; }
        public string NmImprAix { get; set; } = string.Empty;
        public int? CdTOprc { get; set; }
        public int? CdRgrDct { get; set; }
    }
}
