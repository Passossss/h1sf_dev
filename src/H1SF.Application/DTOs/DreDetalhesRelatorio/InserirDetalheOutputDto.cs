using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTOs.DreDetalhesRelatorio
{
    public class InserirDetalheOutputDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool ExecutouInsert { get; set; }
        public int? SequenciaLinha { get; set; }
        public string ChaveComandoPjl { get; set; } = "N"; // WS31-CHV-COMANDO-PJL
        public DateTime DataExecucao { get; set; }
    }
}
