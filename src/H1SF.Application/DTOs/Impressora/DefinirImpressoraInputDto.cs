using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTO
{
    public class DefinirImpressoraInputDto
    {
        public int CdMercDst { get; set; }
        public DateTime DtcSelFtrm { get; set; }
        public string LgonFunc { get; set; } = string.Empty;
    }
}
