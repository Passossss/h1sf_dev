using H1SF.Domain.Entities.DreDetalhesRelatorio;
using H1SF.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Infrastructure.Repositories.DreDetalhesRelatorio
{
    public class DetalheRelatorioRepository : IDetalheRelatorioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetalheRelatorioRepository> _logger;

        public DetalheRelatorioRepository(
            ApplicationDbContext context,
            ILogger<DetalheRelatorioRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> InserirDetalheAsync(DetalheRelatorio detalhe)
        {
            try
            {
                _context.DetalhesRelatorio.Add(detalhe);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Detalhe inserido. ID: {Id}, Sequência: {Sequencia}",
                    detalhe.Id, detalhe.DreCdSqnLnh);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir detalhe");
                return false;
            }
        }

        public async Task<int> ObterProximaSequenciaAsync()
        {
            try
            {
                var maxSequencia = await _context.DetalhesRelatorio
                    .Where(d => d.DreCdSqnPjl != null) // Filtro opcional
                    .MaxAsync(d => (int?)d.DreCdSqnLnh) ?? 0;

                return maxSequencia + 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter próxima sequência");
                return 1;
            }
        }

        public async Task<string> DefinirViaImpressaoAsync()
        {
            // Simulação da rotina 180-00-DEFINE-VIA-IMPRESS
            // Em produção, buscar de configuração ou tabela
            return "01";
        }
    }
}
