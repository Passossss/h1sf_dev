namespace H1SF.Application.DTOs.DefinidorPjl;

/// <summary>
/// Input para 145-00-DEFINE-PJL-OUTBIN-P
/// </summary>
public class DefinidorPjlOutbinP145Input
{
    // WS01-QTDE-VIA-PCK-LIST
    public int QuantidadeViasPackList { get; set; }

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // WS01-CD-SQN-PJL-PKLST-N
    public int CodigoSequenciaPjlPckLst { get; set; }
}

/// <summary>
/// Output para 145-00-DEFINE-PJL-OUTBIN-P
/// </summary>
public class DefinidorPjlOutbinP145Output
{
    public bool Sucesso { get; set; }
    public int IdViaAuxiliar { get; set; }
    public int ComandosPjlGerados { get; set; }
    public string LiteralPrincipal { get; set; } = string.Empty;
}
