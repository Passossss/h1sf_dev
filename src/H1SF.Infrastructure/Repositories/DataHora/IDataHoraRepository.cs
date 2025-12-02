namespace H1SF.Infrastructure.Repositories.DataHora;

/// <summary>
/// Interface para repositório de data/hora do sistema
/// </summary>
public interface IDataHoraRepository
{
    /// <summary>
    /// Obtém data/hora atual do banco no formato YYYYMMDDHH24MISS
    /// SQL: SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') FROM DUAL
    /// </summary>
    Task<string> ObterDataHoraSistemaAsync();
}
