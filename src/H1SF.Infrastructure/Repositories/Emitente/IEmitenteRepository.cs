using H1SF.Domain.Entities.Emitente;

namespace H1SF.Infrastructure.Repositories.Emitente;

/// <summary>
/// 505-00-RECUPERA-EMITENTE - Interface do repositório para recuperação de emitente
/// Linhas COBOL: 3294-3383
/// </summary>
public interface IEmitenteRepository
{
    /// <summary>
    /// Recupera dados completos do emitente
    /// </summary>
    Task<DadosEmitente?> ObterDadosEmitenteAsync(string idCnpj, string locCodigo);
}
