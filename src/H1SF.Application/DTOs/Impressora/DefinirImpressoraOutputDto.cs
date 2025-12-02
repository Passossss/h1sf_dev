using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTO
{
    public class DefinirImpressoraOutputDto
    {
        public string NomeImpressora { get; set; } = string.Empty;
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }
        public string? CodigoErro { get; set; }
    }
}
