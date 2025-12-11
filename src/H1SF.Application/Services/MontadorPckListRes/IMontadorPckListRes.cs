using H1SF.Application.DTOs.MontadorPckList;

namespace H1SF.Application.Services;

/// <summary>
/// 730-00-MONTA-PCK-LIST-RES
/// </summary>
public interface IMontadorPckListRes
{
    Task<MontadorPckListResOutput> ExecutarAsync(MontadorPckListResInput input);
}
