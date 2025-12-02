using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.FaturamentoPws
{
    public class AtualizarPwsOutputDto
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public int ItensAtualizados { get; set; }
        public int VolumesAtualizados { get; set; }
        public DateTime DataExecucao { get; set; }
    }
}
