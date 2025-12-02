namespace H1SF.Domain.Entities.Fabrica;

/// <summary>
/// Tabela H1ST.CONTROLE_VOLUME
/// Armazena informações de controle de volume
/// </summary>
public class ControleVolume
{
    /// <summary>
    /// CD_UF_DST - Código UF Destino
    /// </summary>
    public string? CdUfDst { get; set; }

    /// <summary>
    /// CD_T_MTZ - Código Tipo Motorização
    /// </summary>
    public int? CdTMtz { get; set; }

    /// <summary>
    /// ID_MTZ - Identificador Motorização
    /// </summary>
    public int? IdMtz { get; set; }

    /// <summary>
    /// ID_CLI - Identificador Cliente
    /// </summary>
    public int? IdCli { get; set; }
}
