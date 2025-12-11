namespace H1SF.Application.DTOs.Interface;

/// <summary>
/// Input para 865-00-INTERFACE-DEA-ITEM
/// </summary>
public class InterfaceDeaItem865Input
{
    // SF0005-SFT-CD-T-REC
    public string CodigoTipoRecolhimento { get; set; } = string.Empty;

    // WS36-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0005-SFT-IC-FTRM-TRGD
    public string IndicadorFaturamentoTrigado { get; set; } = string.Empty;

    // WQ01-SYSDATE-DEA
    public string DataSistema { get; set; } = string.Empty;

    // WS01-ID-NUM-CNT
    public string IdNumeroContador { get; set; } = string.Empty;

    // WS01-ID-NUM-T-REG
    public string IdNumeroTotalRegistros { get; set; } = string.Empty;

    // SF0002-ITD-ID-PECA-LOG
    public string IdPeca { get; set; } = string.Empty;

    // CC0001-NUMERO-DEA
    public string NumeroNotaDea { get; set; } = string.Empty;

    // CC0001-SERIE-SUBSERIE-3
    public string SerieSubserie { get; set; } = string.Empty;

    // CC0002-IDF-NUM-4
    public string IdentificacaoNumero4 { get; set; } = string.Empty;

    // ST0005-NM-FNT-ATND
    public string NomeFonteAtendimento { get; set; } = string.Empty;

    // CC0002-NBM-CODIGO-DEA
    public string CodigoNbm { get; set; } = string.Empty;

    // WS01-SIT-TRIB
    public string SituacaoTributaria { get; set; } = string.Empty;

    // CC0002-ENTSAI-UNI-CODIGO
    public string CodigoUnidade { get; set; } = string.Empty;

    // CC0002-QTD-DEA
    public int QuantidadeDea { get; set; }

    // CC0002-PRECO-UNITARIO-DEA
    public decimal PrecoUnitarioDea { get; set; }

    // CC0002-PRECO-TOTAL-CTB-1
    public decimal PrecoTotalContabil { get; set; }

    // CC0002-ALIQ-IPI-LOG-5
    public decimal AliquotaIpi { get; set; }

    // CC0002-ALIQ-ICMS-LOG-5
    public decimal AliquotaIcms { get; set; }

    // WS01-TXT-TX-SERV
    public string TextoTaxaServico { get; set; } = string.Empty;

    // WS01-TXT-PERC-DSCT
    public string TextoPercentualDesconto { get; set; } = string.Empty;

    // CC0002-VL-IPI-DEA
    public decimal ValorIpiDea { get; set; }

    // CC0002-VL-BASE-ICMS-DEA
    public decimal ValorBaseIcmsDea { get; set; }

    // CC0002-VL-ICMS-DEA
    public decimal ValorIcmsDea { get; set; }

    // CC0002-FRETE-DEA
    public decimal FreteDea { get; set; }

    // SF0002-ITD-V-TX-SERV
    public decimal ValorTaxaServico { get; set; }

    // CC0002-DESCONTO-ITEM-DEA
    public decimal DescontoItemDea { get; set; }

    // WS01-PESO-BRT-EMIF-NUM
    public decimal PesoBrutoEmif { get; set; }

    // SF0002-ITD-ID-VOL-LOG
    public string IdVolume { get; set; } = string.Empty;

    // SF0002-ITD-ID-PDD-LOG
    public string IdPdd { get; set; } = string.Empty;

    // ST0005-ID-PSO
    public int IdPso { get; set; }

    // SF0002-ITD-ID-ETIQ-REC-LOG
    public string IdEtiquetaRecolhimento { get; set; } = string.Empty;

    // SF0002-ITD-ID-ETIQ-ACND-LOG
    public string IdEtiquetaAcondicionamento { get; set; } = string.Empty;

    // ST0005-CD-PFO
    public string CodigoPfo { get; set; } = string.Empty;

    // CC0002-CFOP-CODIGO
    public string CodigoCfop { get; set; } = string.Empty;

    // CC0002-NOP-COD-1, CC0002-NOP-COD-2
    public string CodigoNop { get; set; } = string.Empty;

    // ST0005-ID-SQN-ITEM-PDD
    public int IdSequenciaItemPdd { get; set; }

    // ST0005-ID-SQN-ATND-ITEM
    public int IdSequenciaAtendimentoItem { get; set; }

    // ST0005-ID-SQN-OCR-FNT
    public int IdSequenciaOcorrenciaFonte { get; set; }

    // SF0002-ITD-ID-ITM-DE
    public int IdItemDe { get; set; }
}

/// <summary>
/// Output para 865-00-INTERFACE-DEA-ITEM
/// </summary>
public class InterfaceDeaItem865Output
{
    public bool Sucesso { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string IdTipoInterface { get; set; } = string.Empty;
    public string IdSistemaOrigem { get; set; } = string.Empty;
    public string DataGeracao { get; set; } = string.Empty;
    public string NumeroContador { get; set; } = string.Empty;
    public string NumeroTotalRegistros { get; set; } = string.Empty;
    public string IndicadorItemDea { get; set; } = string.Empty;
    public bool InterfaceGravada { get; set; }
}
