using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class ItemRecolhimento
    {
        public int IdEtiqRec { get; set; } // ID_ETIQ_REC
        public int QPecaRec { get; set; }
        public DateTime? DtcFnlItem { get; set; } // DTC_FNL_ITEM - Data finalização do item
        public DateTime? DtcFnlRec { get; set; } // DTC_FNL_REC - Data finalização do recolhimento
        public DateTime DataCriacao { get; set; }
        public string CodigoFonteAtendimento { get; set; } = string.Empty; // CD_FNT_ATND
    }
}
