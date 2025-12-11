namespace H1SF.Application.DTOs.MontadorPckList;

/// <summary>
/// Input para 730-00-MONTA-PCK-LIST-RES
/// </summary>
public class MontadorPckListResInput
{
    // WS01-NUM-VIAS-VOL
    public int NumeroViasVolume { get; set; }

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // WS01-CD-SQN-PJL-PKLST-N
    public int CodigoSequenciaPjlPckLst { get; set; }

    // SF0001-PTD-CD-MERC-DST
    public string PtdCodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0001-PTD-DTC-SEL-FTRM
    public DateTime PtdDataSelecaoFaturamento { get; set; }

    // SF0001-PTD-LGON-FUNC
    public string PtdLoginFuncionario { get; set; } = string.Empty;

    // SF0001-PTD-ID-PTC-DSP
    public int PtdIdProtocoloDespacho { get; set; }

    // SR0005-ID-IDM
    public string IdIdioma { get; set; } = string.Empty;

    // WS01-TITULO-PACK-LIST
    public string TituloPackList { get; set; } = string.Empty;

    // WS01-POS-PACK-LIST-X
    public string PosicaoPackListX { get; set; } = string.Empty;
}

/// <summary>
/// Output para 730-00-MONTA-PCK-LIST-RES
/// </summary>
public class MontadorPckListResOutput
{
    public bool Sucesso { get; set; }
    public int VolumesProcessados { get; set; }
    public int LinhasGeradas { get; set; }
}
