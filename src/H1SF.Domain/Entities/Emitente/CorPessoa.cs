namespace H1SF.Domain.Entities.Emitente;

/// <summary>
/// Tabela B8CC.COR_PESSOA
/// </summary>
public class CorPessoa
{
    /// <summary>
    /// PFJ_CODIGO - Código da Pessoa Física/Jurídica
    /// </summary>
    public string PfjCodigo { get; set; } = string.Empty;

    /// <summary>
    /// MNEMONICO - Razão Social
    /// </summary>
    public string Mnemonico { get; set; } = string.Empty;
}
