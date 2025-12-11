using H1SF.Application.DTOs.MontadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 720-00-MONTA-PCK-LIST
/// </summary>
public interface IMontadorPckList720
{
    Task<MontadorPckList720Output> ExecutarAsync(MontadorPckList720Input input);
}
