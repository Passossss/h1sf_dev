using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTOs.EntradaNfIcRis
{
    public class EnviarInterfaceRisOutputDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public int CdRetrEci { get; set; }
        public int CdRetrAces { get; set; }
        public bool ErroInterface { get; set; }
        public string? MensagemErro { get; set; }
        public string? CdAcesErro { get; set; }
        public string? CdErroEci { get; set; }
        public string? CdErroAces { get; set; }
        public DateTime DataExecucao { get; set; }
        public Guid? IdRequisicao { get; set; }
    }
}
