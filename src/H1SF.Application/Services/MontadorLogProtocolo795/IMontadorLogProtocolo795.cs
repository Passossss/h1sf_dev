using H1SF.Application.DTOs.MontadorLog;

namespace H1SF.Application.Services;

/// <summary>
/// 795-00-MONTA-LOG-PROTOCOLO
/// </summary>
public interface IMontadorLogProtocolo795
{
    Task<MontadorLogProtocolo795Output> ExecutarAsync(MontadorLogProtocolo795Input input);
}
