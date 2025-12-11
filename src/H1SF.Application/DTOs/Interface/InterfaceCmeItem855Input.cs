namespace H1SF.Application.DTOs.Interface;

/// <summary>
/// Input para 855-00-INTERFACE-CME-ITEM
/// </summary>
public class InterfaceCmeItem855Input
{
    // SF0002-ITD-DTC-SEL-FTRM
    public DateTime DataSelecaoFaturamento { get; set; }

    // CC0001-NUMERO-CTB-5
    public string NumeroNotaContabil5 { get; set; } = string.Empty;

    // CC0001-NUMERO-CTB-6
    public string NumeroNotaContabil6 { get; set; } = string.Empty;

    // ST0006-ID-FRN
    public int IdFornecedor { get; set; }

    // CC0002-IDF-NUM-4
    public string IdentificacaoNumero4 { get; set; } = string.Empty;

    // CC0002-PRECO-UNITARIO-CME
    public decimal PrecoUnitarioCme { get; set; }

    // SF0002-ITD-ID-PECA-LOG
    public string IdPeca { get; set; } = string.Empty;

    // SF0002-ITD-ID-PDD-LOG
    public string IdPdd { get; set; } = string.Empty;

    // CC0002-ALIQ-ICMS-LOG
    public decimal AliquotaIcms { get; set; }

    // CC0002-ALIQ-IPI-LOG
    public decimal AliquotaIpi { get; set; }

    // CC0002-VL-ICMS-CME
    public decimal ValorIcmsCme { get; set; }

    // CC0002-VL-IPI-CME
    public decimal ValorIpiCme { get; set; }

    // SF0002-ITD-Q-PECA-FTRD
    public int QuantidadePecaFaturada { get; set; }

    // CC0001-DH-EMISSAO-BARRA
    public string DataEmissaoBarras { get; set; } = string.Empty;
}

/// <summary>
/// Output para 855-00-INTERFACE-CME-ITEM
/// </summary>
public class InterfaceCmeItem855Output
{
    public bool Sucesso { get; set; }
    public string CodigoAcesso { get; set; } = string.Empty;
    public string IdCorrelacao { get; set; } = string.Empty;
    public string CodigoNotaFiscal { get; set; } = string.Empty;
    public string CodigoFornecedor { get; set; } = string.Empty;
    public string TipoMovimento { get; set; } = string.Empty;
    public int TamanhoMensagem { get; set; }
    public bool MensagemEnviada { get; set; }
}
