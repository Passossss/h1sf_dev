namespace H1SF.Application.DTOs.PreparadorPckList;

/// <summary>
/// Input para 757-00-PREPARA-DETALHE-PCK-LST
/// </summary>
public class PreparadorDetalhePckLst757Input
{
    // CB0001-RRE-DTC-GRC
    public DateTime DataGeracao { get; set; }

    // WQ01-DOF-IMPOR-NUMERO
    public string NumeroImportacao { get; set; } = string.Empty;

    // SF0001-PTD-ID-PTC-DSP
    public int IdProtocoloDespacho { get; set; }

    // WS01-NUM-VIAS-VOL
    public int NumeroViasVolume { get; set; }

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // WS01-CD-SQN-PJL-PKLST-N
    public int CodigoSequenciaPjlPckLst { get; set; }
}

/// <summary>
/// Output para 757-00-PREPARA-DETALHE-PCK-LST
/// </summary>
public class PreparadorDetalhePckLst757Output
{
    public bool Sucesso { get; set; }
    public string CodigoSistema { get; set; } = string.Empty;
    public DateTime DataGeracao { get; set; }
    public string IdPrincipal { get; set; } = string.Empty;
    public int CodigoSequenciaDocumento { get; set; }
    public string LiteralPrincipal { get; set; } = string.Empty;
    public int CodigoSequenciaPjl { get; set; }
}
