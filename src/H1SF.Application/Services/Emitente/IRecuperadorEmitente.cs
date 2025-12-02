using H1SF.Domain.Entities.Emitente;

namespace H1SF.Application.Services.Emitente;

/// <summary>
/// 505-00-RECUPERA-EMITENTE - Interface do serviço de recuperação de emitente
/// Linhas COBOL: 3294-3383
/// </summary>
public interface IRecuperadorEmitente
{
    /// <summary>
    /// Recupera dados completos do emitente a partir do CNPJ da fábrica
    /// </summary>
    Task<DadosEmitente> RecuperarEmitenteAsync(string idCnpj);
}
