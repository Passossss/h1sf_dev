using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    /// <summary>
    /// Tabela H1SF.SFT_SELECAO_FTRM
    /// </summary>
    public class SelecaoFaturamento
    {
        public int SftCdTRec { get; set; }
        public int SftIdImprFtrm { get; set; }
        public string SftIcFtrmTrgd { get; set; } = string.Empty;
        public string SftIcNaczIcpnBt { get; set; } = "N";
        public int SftCdMercDstOrig { get; set; }
        public DateTime SftDtcSelFtrmOrig { get; set; }
        public string SftLgonFuncOrig { get; set; } = string.Empty;
        public string SftCdMercDst { get; set; }
        public DateTime SftDtcSelFtrm { get; set; }
        public string SftLgonFunc { get; set; } = string.Empty;
        
        // Campos adicionais para query 572-20-SQL-CNPJ-TRIANG
        public int? SftCdTMtz { get; set; }
        public int? SftIdMtz { get; set; }
        public int? SftIdCli { get; set; }
    }
}
