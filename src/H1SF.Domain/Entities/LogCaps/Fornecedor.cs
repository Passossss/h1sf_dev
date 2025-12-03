namespace H1SF.Domain.Entities.LogCaps;

/// <summary>
/// Entidade SUP_FORNECEDOR (H1SF schema)
/// </summary>
public class Fornecedor
{
    public string CodigoFonteAtendimento { get; set; } = string.Empty; // SUP_CD_FNT_ATND
    public string CodigoFornecedorSuprimentos { get; set; } = string.Empty; // SUP_CD_FORN_SPR
}
