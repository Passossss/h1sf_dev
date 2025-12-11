using H1SF.Application.DTOs.DescarregaVias;

namespace H1SF.Application.Services;

/// <summary>
/// 820-00-DESCARREGA-VIAS-PCK-LST
/// </summary>
public interface IDescarregaViasPckLst
{
    Task<DescarregaViasPckLstOutput> ExecutarAsync(DescarregaViasPckLstInput input);
}
