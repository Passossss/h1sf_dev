using H1SF.Application.DTOs.PreparadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 757-00-PREPARA-DETALHE-PCK-LST
/// </summary>
public interface IPreparadorDetalhePckLst757
{
    Task<PreparadorDetalhePckLst757Output> ExecutarAsync(PreparadorDetalhePckLst757Input input);
}
