namespace H1SF.Domain.Entities.Fabrica;

/// <summary>
/// Tabela H1CB.FABRICA
/// Armazena informações de fábricas
/// </summary>
public class Fabrica
{
    /// <summary>
    /// CD_T_PRD - Código Tipo Produto
    /// </summary>
    public string CdTPrd { get; set; } = string.Empty;

    /// <summary>
    /// ID_CNPJ - CNPJ da Fábrica
    /// </summary>
    public string IdCnpj { get; set; } = string.Empty;

    /// <summary>
    /// CD_FBR - Código Fábrica
    /// </summary>
    public int CdFbr { get; set; }

    /// <summary>
    /// CD_UF - Código UF
    /// </summary>
    public string? CdUf { get; set; }

    /// <summary>
    /// IC_FBR_TRGD - Indicador Fábrica Triangulação
    /// </summary>
    public string? IcFbrTrgd { get; set; }

    /// <summary>
    /// ID_CLI - Identificador Cliente
    /// </summary>
    public int? IdCli { get; set; }
}
