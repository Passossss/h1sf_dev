namespace H1SF.Domain.Entities.Protocolo;

/// <summary>
/// DTO para retorno da consulta 500-00-LE-PROTOCOLO / 705-00-MONTA-DANFE
/// </summary>
public class DadosProtocolo
{
    public int PtdCdMercDst { get; set; }
    public DateTime PtdDtcSelFtrm { get; set; }
    public string PtdLgonFunc { get; set; } = string.Empty;
    public int PtdCdSeq { get; set; }
    public int? PtdCdTRec { get; set; }
    public int? PtdCdTMtz { get; set; }
    public int? PtdIdMtz { get; set; }
    public int? PtdIdCli { get; set; }
    public string PtdIdPtcDsp { get; set; } = string.Empty;
    
    /// <summary>
    /// PTD_ID_PTC_DSP formatado: LTRIM(TO_CHAR(TO_NUMBER(PTD_ID_PTC_DSP),'9,999,999'))
    /// </summary>
    public string PtdIdPtcDspFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// PTD_DTC_PTC_DSP formatado: TO_CHAR(PTD_DTC_PTC_DSP,'DD/MM/YYYY - HH24') || 'h' || TO_CHAR(PTD_DTC_PTC_DSP,'MI')
    /// </summary>
    public string PtdDtcPtcDspFormatado { get; set; } = string.Empty;
    
    public string? PtdCdPgtFrt { get; set; }
    public string? PtdCdTrsr { get; set; }
    
    /// <summary>
    /// Peso líquido em Kg formatado com 'Kg': LTRIM(TO_CHAR(TO_NUMBER(PTD_PESO_TTL_LQD)/1000,'9,999,999,990.000') || 'Kg')
    /// </summary>
    public string PesoLiquidoFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// Peso líquido numérico em Kg
    /// </summary>
    public decimal PesoLiquidoKg { get; set; }
    
    /// <summary>
    /// Peso bruto em Kg formatado com 'Kg'
    /// </summary>
    public string PesoBrutoFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// Peso bruto numérico em Kg
    /// </summary>
    public decimal PesoBrutoKg { get; set; }
    
    /// <summary>
    /// Peso embalagem (diferença bruto - líquido) formatado
    /// </summary>
    public string PesoEmbalagemFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// Peso embalagem numérico em Kg
    /// </summary>
    public decimal PesoEmbalagemKg { get; set; }
    
    /// <summary>
    /// Quantidade total volumes formatado
    /// </summary>
    public string QuantidadeVolumesFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// Quantidade total volumes numérico
    /// </summary>
    public int QuantidadeVolumes { get; set; }
    
    /// <summary>
    /// Valor total mercadoria formatado: 'R$' || TO_CHAR(TO_NUMBER(PTD_V_TTL_MRCD)/100,'99,999,999,990.00')
    /// </summary>
    public string ValorTotalFormatado { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor total mercadoria numérico em reais
    /// </summary>
    public decimal ValorTotalReais { get; set; }
    
    public string PtdIcDspImps { get; set; } = "N";
}
