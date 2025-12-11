namespace H1SF.Application.DTOs.InterfaceSdeFat;

/// <summary>
/// Input para 885-00-INTERFACE-S-DE-FAT
/// </summary>
public class InterfaceSdeFatInput
{
    // ST0001-CD-REGR-FTRM
    public string CodigoRegraFaturamento { get; set; } = string.Empty;

    // WQ01-SYSDATE-S
    public string DataSistema { get; set; } = string.Empty;

    // WS01-ID-NUM-CNT-S
    public string IdNumeroContador { get; set; } = string.Empty;

    // SF0001-PTD-DTC-SEL-FTRM
    public DateTime DataSelecaoFaturamento { get; set; }

    // SF0002-ITD-FTR-EXP
    public string FaturaExportacao { get; set; } = string.Empty;

    // SF0001-PTD-ID-CLI
    public int IdCliente { get; set; }

    // WS01-Q-TTL-ITEM-FAT
    public int QuantidadeTotalItemFaturado { get; set; }

    // SF0001-PTD-CD-T-REC
    public string CodigoTipoRecolhimento { get; set; } = string.Empty;

    // SF0002-ITD-CD-MOD-TRSP-LOG
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;

    // SF0002-ITD-ID-PDD-LOG
    public string IdPdd { get; set; } = string.Empty;

    // SF0002-ITD-ID-FTR-API-LOG
    public string IdFaturaApi { get; set; } = string.Empty;
}

/// <summary>
/// Output para 885-00-INTERFACE-S-DE-FAT
/// </summary>
public class InterfaceSdeFatOutput
{
    public bool Sucesso { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string IdTipoInterface { get; set; } = string.Empty;
    public string IdSistemaOrigem { get; set; } = string.Empty;
    public string DataGeracao { get; set; } = string.Empty;
    public string NumeroContador { get; set; } = string.Empty;
    public string CodigoTipoRecolhimentoProcessado { get; set; } = string.Empty;
    public bool InterfaceGravada { get; set; }
}
