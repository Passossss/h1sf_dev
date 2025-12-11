namespace H1SF.Application.DTOs.MontadorPckList;

/// <summary>
/// Input para 720-00-MONTA-PCK-LIST
/// </summary>
public class MontadorPckList720Input
{
    // WS01-NUM-VIAS-VOL
    public int NumeroViasVolume { get; set; }

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // WS01-CD-SQN-PJL-PKLST-N
    public int CodigoSequenciaPjlPckLst { get; set; }

    // SF0002-ITD-FTR-EXP
    public string FaturaExportacao { get; set; } = string.Empty;

    // SF0001-PTD-CD-MERC-DST
    public string PtdCodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0001-PTD-DTC-SEL-FTRM
    public DateTime PtdDataSelecaoFaturamento { get; set; }

    // SF0001-PTD-LGON-FUNC
    public string PtdLoginFuncionario { get; set; } = string.Empty;

    // SF0001-PTD-ID-PTC-DSP
    public int PtdIdProtocoloDespacho { get; set; }

    // WS31-CHV-DEA
    public string ChaveDea { get; set; } = string.Empty;

    // SR0002-ID-PARM (parametro 00333 ou 00334)
    public string ParametroTitulo { get; set; } = string.Empty;
}

/// <summary>
/// Output para 720-00-MONTA-PCK-LIST
/// </summary>
public class MontadorPckList720Output
{
    public bool Sucesso { get; set; }
    public string IdIdioma { get; set; } = string.Empty;
    public string TituloPackList { get; set; } = string.Empty;
    public string PosicaoPackListX { get; set; } = string.Empty;
    public int VolumesProcessados { get; set; }
    public int LinhasGeradas { get; set; }
}
