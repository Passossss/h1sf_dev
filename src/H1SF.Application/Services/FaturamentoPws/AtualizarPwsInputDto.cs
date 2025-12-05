using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.FaturamentoPws
{
    public class AtualizarPwsInputDto
    {
        public string CdMercDst { get; set; }
        public DateTime DtcSelFtrm { get; set; }
        public string LgonFunc { get; set; } = string.Empty;
        public string FaseFtrm { get; set; } = "1";
        public string? CdMercDstW36 { get; set; }
        public string? FaseFtrmW36 { get; set; }
        public string? SftIcNaczIcpnBt { get; set; }
        public string? SftCdTRec { get; set; }
        public string? SftIcFtrmTrgd { get; set; }
        public string? CdRegrFtrm { get; set; }
    }
}
