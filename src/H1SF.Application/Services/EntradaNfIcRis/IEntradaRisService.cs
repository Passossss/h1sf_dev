using H1SF.Application.DTOs.EntradaNfIcRis;

namespace H1SF.Application.Services.EntradaNfIcRis
{
    public interface IEntradaRisService
    {
        Task<EnviarInterfaceRisOutputDto> EnviarParaInterfaceRisAsync(EnviarInterfaceRisInputDto input);
        Task<EnviarInterfaceRisOutputDto> ExecutarEntradaNfIcRisAsync(EnviarInterfaceRisInputDto input);
    }
}
