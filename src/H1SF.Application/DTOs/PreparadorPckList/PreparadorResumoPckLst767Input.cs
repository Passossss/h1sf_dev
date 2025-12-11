namespace H1SF.Application.DTOs.PreparadorPckList;

/// <summary>
/// Input para 767-00-PREPARA-RESUMO-PCK-LST
/// </summary>
public class PreparadorResumoPckLst767Input
{
    // WQ01-DOF-IMPOR-NUMERO
    public string NumeroImportacao { get; set; } = string.Empty;

    // SF0001-PTD-CD-T-REC
    public string CodigoTipoRecolhimento { get; set; } = string.Empty;

    // SF0001-PTD-ID-MTZ
    public int IdMatriz { get; set; }

    // SF0001-PTD-ID-CLI
    public int IdCliente { get; set; }

    // SF0001-PTD-ID-PTC-DSP
    public int IdProtocoloDespacho { get; set; }

    // WS01-IMPRESSORA-LASER
    public string IdImpressora { get; set; } = string.Empty;
}

/// <summary>
/// Output para 767-00-PREPARA-RESUMO-PCK-LST
/// </summary>
public class PreparadorResumoPckLst767Output
{
    public bool Sucesso { get; set; }
    public string CodigoSistema { get; set; } = string.Empty;
    public string IdPrincipal { get; set; } = string.Empty;
    public string LiteralPrincipal { get; set; } = string.Empty;
    public string IdAuxiliar1 { get; set; } = string.Empty;
    public int IdAuxiliar2 { get; set; }
    public int IdAuxiliar3 { get; set; }
    public int IdAuxiliar4 { get; set; }
    public string IdRelatorio { get; set; } = string.Empty;
    public string IdImpressora { get; set; } = string.Empty;
}
