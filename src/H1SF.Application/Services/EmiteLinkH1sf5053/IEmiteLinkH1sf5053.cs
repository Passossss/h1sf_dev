using H1SF.Application.DTOs.EmiteLinkH1sf5053;

namespace H1SF.Application.Services.EmiteLinkH1sf5053;

/// <summary>
/// Interface para 583-00-EMITE-LINK-H1SF5053
/// </summary>
public interface IEmiteLinkH1sf5053
{
    /// <summary>
    /// Executa link para programa H1SF5053
    /// </summary>
    Task<EmiteLinkH1sf5053Output> ExecutarAsync(EmiteLinkH1sf5053Input input);
}
