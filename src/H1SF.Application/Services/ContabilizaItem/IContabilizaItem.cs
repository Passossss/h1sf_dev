using H1SF.Application.DTOs.ContabilizaItem;

namespace H1SF.Application.Services.ContabilizaItem;

/// <summary>
/// Interface para 845-00-CONTABILIZA-ITEM
/// </summary>
public interface IContabilizaItem
{
    /// <summary>
    /// Executa contabilização de item do DANFE
    /// </summary>
    Task<ContabilizaItemOutput> ExecutarAsync(ContabilizaItemInput input);
}
