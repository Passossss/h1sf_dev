namespace H1SF.Application.DTOs.ContabilizaItem;

/// <summary>
/// Input para 845-00-CONTABILIZA-ITEM
/// </summary>
public class ContabilizaItemInput
{
    // SF0002-ITD-DTC-SEL-FTRM
    public DateTime DataSelecaoFaturamento { get; set; }

    // CC0001-NUMERO-CTB-5, CC0001-NUMERO-CTB-6
    public string NumeroNotaContabil { get; set; } = string.Empty;

    // CC0001-DH-EMISSAO-I-SEC
    public long DataEmissaoSegundos { get; set; }

    // SR0003-ID-FRN
    public int IdFornecedor { get; set; }

    // CC0004-CPF-CGC
    public string CpfCgc { get; set; } = string.Empty;

    // CC0002-IDF-NUM
    public string IdentificacaoNumero { get; set; } = string.Empty;

    // SF0002-ITD-ID-PECA-LOG
    public string IdPeca { get; set; } = string.Empty;

    // CC0002-NBM-CODIGO
    public string CodigoNbm { get; set; } = string.Empty;

    // CC0002-ALIQ-IPI-LOG, CC0002-ALIQ-ICMS-LOG
    public decimal AliquotaIpi { get; set; }
    public decimal AliquotaIcms { get; set; }

    // WQ01-NOP-CODIGO-AUX
    public string CodigoNop { get; set; } = string.Empty;

    // CC0002-CFOP-CODIGO
    public string CodigoCfop { get; set; } = string.Empty;

    // CT0001-CD-AUX-ICM, CT0001-CD-AUX-IPI
    public string CodigoAuxiliarIcm { get; set; } = string.Empty;
    public string CodigoAuxiliarIpi { get; set; } = string.Empty;

    // SF0002-ITD-Q-PECA-FTRD-CTB
    public int QuantidadePecaFaturada { get; set; }

    // WS01-PESO-BRT-EMIF-NUM-R
    public decimal PesoBruto { get; set; }

    // CC0002-PRECO-UNITARIO-CTB, CC0002-PRECO-TOTAL-CTB
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal1 { get; set; }
    public decimal PrecoTotal2 { get; set; }

    // CC0002-VL-BASE-IPI, CC0002-VL-BASE-ICMS
    public decimal ValorBaseIpi { get; set; }
    public decimal ValorBaseIcms { get; set; }

    // CC0002-VL-TRIBUTAVEL-STF
    public decimal ValorTributavelStf { get; set; }

    // CC0002-VL-IPI-CTB, CC0002-VL-ICMS-CTB, CC0002-VL-STF-CTB
    public decimal ValorIpi { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal ValorStf { get; set; }

    // CC0002-ALIQ-IPI-CTB
    public decimal AliquotaIpiCtb { get; set; }

    // CT0001-DEB-CD-CT, CT0001-DEB-CD-SUB1, CT0001-DEB-CD-SUB2
    public string CodigoContaDebito { get; set; } = string.Empty;
    public string CodigoSub1Debito { get; set; } = string.Empty;
    public string CodigoSub2Debito { get; set; } = string.Empty;

    // SR0003-ID-NF-CTB
    public string IdNotaFiscalContabil { get; set; } = string.Empty;

    // CC0002-IDF-NUM-CTB
    public string IdentificacaoNumeroCtb { get; set; } = string.Empty;

    // ST0005-CD-GR-PECA
    public string CodigoGrupoPeca { get; set; } = string.Empty;
}

/// <summary>
/// Output para 845-00-CONTABILIZA-ITEM
/// </summary>
public class ContabilizaItemOutput
{
    public bool Sucesso { get; set; }
    public string CodigoAcesso { get; set; } = string.Empty;
    public string CodigoRetornoEci { get; set; } = string.Empty;
    public string CodigoRetornoAcesso { get; set; } = string.Empty;
    public string IdMensagemCorrelacao { get; set; } = string.Empty;
    public string CodigoRr { get; set; } = string.Empty;
    public int ContadorItensContabilizados { get; set; }
    public bool MensagemEnviada { get; set; }
}
