namespace H1SF.Domain.Entities.Fabrica;

/// <summary>
/// DTO para retorno da consulta 572-00-SQL-RECUPERA-CNPJ
/// </summary>
public class CnpjFabrica
{
    /// <summary>
    /// CD_T_PRD - Código Tipo Produto
    /// </summary>
    public string CdTPrd { get; set; } = string.Empty;

    /// <summary>
    /// ID_CNPJ - CNPJ da Fábrica (PIC X(014))
    /// </summary>
    public string IdCnpj { get; set; } = string.Empty;

    /// <summary>
    /// CD_FBR - Código Fábrica
    /// </summary>
    public int CdFbr { get; set; }
}
