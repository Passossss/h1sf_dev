using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class ItemRecolhimento
    {
        public int IdEtiqRec { get; set; }
        public int QPecaRec { get; set; }
        public DateTime? DtcFnlItem { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
