using H1SF.Application.DTOs.Relatorio;

namespace H1SF.Application.Services;

/// <summary>
/// 550-00-GRAVA-RESUMO-REL
/// </summary>
public interface IGravaResumoRel
{
    Task<GravaResumoRelOutput> ExecutarAsync(GravaResumoRelInput input);
}
