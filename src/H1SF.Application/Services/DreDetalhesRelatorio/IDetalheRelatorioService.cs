using H1SF.Application.DTOs.DreDetalhesRelatorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.DreDetalhesRelatorio
{
    public interface IDetalheRelatorioService
    {
        Task<InserirDetalheOutputDto> ExecutarInsercaoDetalheAsync(InserirDetalheInputDto input);
    }
}
