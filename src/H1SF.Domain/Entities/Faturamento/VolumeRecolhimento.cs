using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    public class VolumeRecolhimento
    {
        public int IdVol { get; set; }
        public string? IdDctFscl { get; set; }
        public string? IcVolFtrd { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
