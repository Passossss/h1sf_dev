using H1SF.Domain.Entities.DreDetalhesRelatorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Infrastructure.Repositories.DreDetalhesRelatorio
{
    public interface IDetalheRelatorioRepository
    {
        Task<bool> InserirDetalheAsync(DetalheRelatorio detalhe);
        Task<int> ObterProximaSequenciaAsync();
        Task<string> DefinirViaImpressaoAsync();
    }
}
