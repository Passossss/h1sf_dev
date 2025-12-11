namespace H1SF.Application.DTOs.Interface;

/// <summary>
/// Input para 880-00-INTERFACE-S-DE-ITEM
/// </summary>
public class InterfaceSDeItem880Input
{
    // CC0002-QTD-DEA
    public int QuantidadeDea { get; set; }

    // SF0001-PTD-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0001-PTD-DTC-SEL-FTRM
    public DateTime DataSelecaoFaturamento { get; set; }

    // SF0001-PTD-LGON-FUNC
    public string LoginFuncionario { get; set; } = string.Empty;

    // SF0002-ITD-ID-PECA-LOG
    public string IdPeca { get; set; } = string.Empty;

    // ST0001-CD-REGR-FTRM
    public string CodigoRegraFaturamento { get; set; } = string.Empty;

    // WQ01-SYSDATE-S
    public string DataSistema { get; set; } = string.Empty;

    // WS01-ID-NUM-CNT-S
    public string IdNumeroContador { get; set; } = string.Empty;

    // WS01-ID-NUM-T-REG-S
    public string IdNumeroTotalRegistros { get; set; } = string.Empty;

    // WQ02-CD-MERC-DST
    public string WqCodigoMercadoriaDestino { get; set; } = string.Empty;

    // WQ02-DTC-SEL-FTRM
    public DateTime WqDataSelecaoFaturamento { get; set; }

    // WQ02-LGON-FUNC
    public string WqLoginFuncionario { get; set; } = string.Empty;

    // SF0002-ITD-FTR-EXP
    public string FaturaExportacao { get; set; } = string.Empty;

    // SF0002-ITD-ID-ETIQ-ACND-LOG
    public string IdEtiquetaAcondicionamento { get; set; } = string.Empty;

    // CC0001-NUMERO
    public string NumeroNota { get; set; } = string.Empty;

    // CC0002-IDF-NUM
    public string IdentificacaoNumero { get; set; } = string.Empty;

    // CC0002-NBM-CODIGO-DEA
    public string CodigoNbm { get; set; } = string.Empty;

    // CC0002-PRECO-UNIT-DEA-US-X
    public decimal PrecoUnitarioDeaUsd { get; set; }

    // CC0002-PRECO-TOT-DEA-US-X
    public decimal PrecoTotalDeaUsd { get; set; }

    // SF0002-ITD-ID-VOL-LOG
    public string IdVolume { get; set; } = string.Empty;

    // SF0002-ITD-ID-ETIQ-REC-LOG
    public string IdEtiquetaRecolhimento { get; set; } = string.Empty;

    // SF0002-ITD-ID-ITM-DE
    public int IdItemDe { get; set; }

    // ST0005-CD-PFO
    public string CodigoPfo { get; set; } = string.Empty;

    // SF0002-ITD-V-US-RT-FRT
    public decimal ValorFreteUsd { get; set; }

    // SF0002-ITD-V-US-RT-SGR
    public decimal ValorSeguroUsd { get; set; }

    // SF0002-ITD-V-US-RT-ODA
    public decimal ValorOutrasDespesasUsd { get; set; }

    // SF0002-ITD-V-US-ARED-CES-ODA
    public decimal ValorAredondamentoCesOda { get; set; }

    // SF0002-ITD-V-AI-CHRG-API
    public decimal ValorAiChrgApi { get; set; }
}

/// <summary>
/// Output para 880-00-INTERFACE-S-DE-ITEM
/// </summary>
public class InterfaceSDeItem880Output
{
    public bool Sucesso { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string IdTipoInterface { get; set; } = string.Empty;
    public string IdSistemaOrigem { get; set; } = string.Empty;
    public string DataGeracao { get; set; } = string.Empty;
    public decimal QuantidadeTotalItemFaturado { get; set; }
    public decimal PesoLiquidoTotal { get; set; }
    public decimal TaxaCambio { get; set; }
    public bool InterfaceGravada { get; set; }
}
