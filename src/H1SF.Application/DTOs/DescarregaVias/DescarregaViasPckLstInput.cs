namespace H1SF.Application.DTOs.DescarregaVias;

/// <summary>
/// Input para 820-00-DESCARREGA-VIAS-PCK-LST
/// </summary>
public class DescarregaViasPckLstInput
{
    // WS01-NUM-VIAS-VOL
    public int NumeroViasVolume { get; set; }

    // WS32-IND-IMPR
    public int IndiceImpressao { get; set; }

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // WS01-CD-SQN-PJL-PKLST-N
    public int CodigoSequenciaPjlPckLst { get; set; }

    // WS01-LIN-IMPR
    public string[] LinhasImpressao { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Output para 820-00-DESCARREGA-VIAS-PCK-LST
/// </summary>
public class DescarregaViasPckLstOutput
{
    public bool Sucesso { get; set; }
    public int NumeroViasProcessadas { get; set; }
}
