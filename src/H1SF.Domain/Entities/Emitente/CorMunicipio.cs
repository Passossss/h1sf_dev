namespace H1SF.Domain.Entities.Emitente;

/// <summary>
/// Tabela B8CC.COR_MUNICIPIO
/// </summary>
public class CorMunicipio
{
    /// <summary>
    /// MUN_CODIGO - Código do Município
    /// </summary>
    public string MunCodigo { get; set; } = string.Empty;

    /// <summary>
    /// UF_CODIGO - Código da UF
    /// </summary>
    public string UfCodigo { get; set; } = string.Empty;
}
