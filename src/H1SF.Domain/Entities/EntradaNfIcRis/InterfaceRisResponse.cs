using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.EntradaNfIcRis
{
    public class InterfaceRisResponse
    {
        public int CdRetrEci { get; set; }          // WX04-CD-RETR-ECI
        public int CdRetrAces { get; set; }         // WX04-CD-RETR-ACES
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }
        public DateTime DataResposta { get; set; }
    }
}
