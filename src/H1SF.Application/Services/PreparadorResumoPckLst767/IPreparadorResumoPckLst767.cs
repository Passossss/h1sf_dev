using H1SF.Application.DTOs.PreparadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 767-00-PREPARA-RESUMO-PCK-LST
/// </summary>
public interface IPreparadorResumoPckLst767
{
    Task<PreparadorResumoPckLst767Output> ExecutarAsync(PreparadorResumoPckLst767Input input);
}
