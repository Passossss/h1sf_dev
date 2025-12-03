using H1SF.Domain.Entities.EntradaNfIcRis;

namespace H1SF.Infrastructure.Repositories.EntradaNfIcRis
{
    public interface IEntradaRisRepository
{
    Task<Guid> SalvarRequisicaoAsync(EntradaRisRequest request);
    Task AtualizarRespostaAsync(int id, EntradaRisResponse response);
    Task<EntradaRisResponse> EmitirLinkParaInterfaceRisAsync(EntradaRisRequest request);
    Task<EntradaRisRequest?> ObterRequisicaoPorIdAsync(int id);
}
}
