using H1SF.Domain.Entities.EntradaNfIcRis;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace H1SF.Infrastructure.Repositories.EntradaNfIcRis
{
    public class InterfaceRisRepository : IInterfaceRisRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InterfaceRisRepository> _logger;

        public InterfaceRisRepository(
            ApplicationDbContext context,
            ILogger<InterfaceRisRepository> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogInformation("Interface RIS Repository inicializado (modo MOCK)");
        }

        public async Task<Guid> SalvarRequisicaoAsync(InterfaceRisRequest request)
        {
            try
            {
                _context.InterfaceRisRequests.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Requisição RIS salva. ID: {Id}", request.Id);

                return Guid.NewGuid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar requisição RIS");
                throw;
            }
        }

        public async Task AtualizarRespostaAsync(int id, InterfaceRisResponse response)
        {
            try
            {
                var request = await _context.InterfaceRisRequests
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (request != null)
                {
                    request.CdRetrEci = response.CdRetrEci;
                    request.CdRetrAces = response.CdRetrAces;
                    request.Sucesso = response.Sucesso;
                    request.MensagemErro = response.MensagemErro;

                    await _context.SaveChangesAsync();

                    _logger.LogDebug("Resposta RIS atualizada. ID: {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar resposta RIS");
            }
        }

        public async Task<InterfaceRisResponse> EmitirLinkParaInterfaceRisAsync(InterfaceRisRequest request)
        {
            _logger.LogInformation("Emitindo link MOCK para interface RIS - Programa H1SF5008");

            return await EmitirLinkMockAsync(request);
        }

        private async Task<InterfaceRisResponse> EmitirLinkMockAsync(InterfaceRisRequest request)
        {
            try
            {
                // Simulação de latência
                await Task.Delay(100);

                // Mock SIMPLES - sempre sucesso para começar
                // Depois pode adicionar lógica mais complexa
                return new InterfaceRisResponse
                {
                    CdRetrEci = 0,
                    CdRetrAces = 0,
                    Sucesso = true,
                    MensagemErro = null,
                    DataResposta = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no mock da interface RIS");
                return new InterfaceRisResponse
                {
                    CdRetrEci = 99,
                    CdRetrAces = 99,
                    Sucesso = false,
                    MensagemErro = $"Erro interno no mock: {ex.Message}",
                    DataResposta = DateTime.Now
                };
            }
        }

        public async Task<InterfaceRisRequest?> ObterRequisicaoPorIdAsync(int id)
        {
            return await _context.InterfaceRisRequests
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // Método opcional para testes - pode remover se não for usar
        public void ConfigurarModoMock(bool usarMock)
        {
            // Implementação vazia por enquanto
            _logger.LogDebug("ConfigurarModoMock chamado com: {UsarMock}", usarMock);
        }
    }
}