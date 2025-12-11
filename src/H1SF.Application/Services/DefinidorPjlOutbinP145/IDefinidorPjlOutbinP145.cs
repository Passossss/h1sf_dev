using H1SF.Application.DTOs.DefinidorPjl;

namespace H1SF.Application.Services;

/// <summary>
/// 145-00-DEFINE-PJL-OUTBIN-P
/// </summary>
public interface IDefinidorPjlOutbinP145
{
    Task<DefinidorPjlOutbinP145Output> ExecutarAsync(DefinidorPjlOutbinP145Input input);
}
