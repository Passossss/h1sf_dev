namespace H1SF.Application.DTOs.MontadorLog;

/// <summary>
/// Input para 795-00-MONTA-LOG-PROTOCOLO
/// </summary>
public class MontadorLogProtocolo795Input
{
    // SF0001-PTD-CD-T-REC
    public string CodigoTipoRecolhimento { get; set; } = string.Empty;

    // SF0001-PTD-CD-T-MTZ
    public string CodigoTipoMatriz { get; set; } = string.Empty;

    // SF0001-PTD-CD-MERC-DST
    public string CodigoMercadoriaDestino { get; set; } = string.Empty;

    // SF0001-PTD-DTC-SEL-FTRM
    public DateTime DataSelecaoFaturamento { get; set; }

    // SF0001-PTD-LGON-FUNC
    public string LoginFuncionario { get; set; } = string.Empty;

    // SF0001-PTD-ID-PTC-DSP
    public int IdProtocoloDespacho { get; set; }

    // CC0001-NUMERO
    public string NumeroNota { get; set; } = string.Empty;

    // WQ02-CD-MERC-DST
    public string WqCodigoMercadoriaDestino { get; set; } = string.Empty;

    // WQ02-DTC-SEL-FTRM
    public DateTime WqDataSelecaoFaturamento { get; set; }

    // WQ02-LGON-FUNC
    public string WqLoginFuncionario { get; set; } = string.Empty;

    // WQ01-ID-CLI-AUX
    public int IdCliente { get; set; }

    // SF0001-PTD-ID-MTZ
    public int IdMatriz { get; set; }

    // SF0001-PTD-ID-CLI
    public int PtdIdCliente { get; set; }

    // SF0001-PTD-CD-PGT-FRT
    public string CodigoPagamentoFrete { get; set; } = string.Empty;

    // SF0001-PTD-CD-TRSR
    public string CodigoTransportador { get; set; } = string.Empty;

    // CC0005-RAZAO-SOCIAL-E
    public string RazaoSocialTransportador { get; set; } = string.Empty;

    // SF0001-PTD-Q-TTL-VOL-LOG
    public string QuantidadeTotalVolumes { get; set; } = string.Empty;

    // SF0001-PTD-PESO-TTL-LQD-LOG
    public decimal PesoTotalLiquido { get; set; }

    // SF0001-PTD-PESO-TTL-EMB-LOG
    public decimal PesoTotalEmbalagem { get; set; }

    // SF0001-PTD-PESO-TTL-BRT-LOG
    public decimal PesoTotalBruto { get; set; }

    // SF0001-PTD-V-TTL-MRCD-LOG
    public decimal ValorTotalMercadoria { get; set; }

    // WS36-FASE-FTRM
    public string FaseFaturamento { get; set; } = string.Empty;

    // ST0001-CD-REGR-FTRM
    public string CodigoRegraFaturamento { get; set; } = string.Empty;
}

/// <summary>
/// Output para 795-00-MONTA-LOG-PROTOCOLO
/// </summary>
public class MontadorLogProtocolo795Output
{
    public bool Sucesso { get; set; }
    public string CodigoAcesso { get; set; } = string.Empty;
    public string CodigoModalidadeTransporte { get; set; } = string.Empty;
    public int IdMatriz { get; set; }
    public int IdCliente { get; set; }
    public string IdAgrupamento { get; set; } = string.Empty;
    public int IdProtocoloDespacho { get; set; }
    public string IdFaturaExportacao { get; set; } = string.Empty;
    public string CodigoPagamentoFrete { get; set; } = string.Empty;
    public bool MensagemEnviada { get; set; }
}
