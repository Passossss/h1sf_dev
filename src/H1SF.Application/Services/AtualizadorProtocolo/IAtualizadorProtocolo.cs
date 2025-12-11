using H1SF.Application.DTOs.Protocolo;

namespace H1SF.Application.Services;

/// <summary>
/// 540-00-ATUALIZA-PROTOCOLO
/// </summary>
public interface IAtualizadorProtocolo
{
    Task<AtualizadorProtocoloOutput> ExecutarAsync(AtualizadorProtocoloInput input);
}
