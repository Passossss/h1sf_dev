using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class ItemVolumeRecolhimento
    {
        public int IdEtiqRec { get; set; }
        public int IdVol { get; set; }
        public DateTime DataAssociacao { get; set; }
    }
}
