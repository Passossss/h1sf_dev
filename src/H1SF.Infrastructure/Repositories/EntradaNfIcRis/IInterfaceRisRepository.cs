using H1SF.Domain.Entities.EntradaNfIcRis;

namespace H1SF.Infrastructure.Repositories.EntradaNfIcRis
{
    public interface IInterfaceRisRepository
{
    Task<Guid> SalvarRequisicaoAsync(InterfaceRisRequest request);
    Task AtualizarRespostaAsync(int id, InterfaceRisResponse response);
    Task<InterfaceRisResponse> EmitirLinkParaInterfaceRisAsync(InterfaceRisRequest request);
    Task<InterfaceRisRequest?> ObterRequisicaoPorIdAsync(int id);
}
}
