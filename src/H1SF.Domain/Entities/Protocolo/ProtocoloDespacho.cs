namespace H1SF.Domain.Entities.Protocolo;

/// <summary>
/// Tabela H1SF.PTD_PROTODSP
/// </summary>
public class ProtocoloDespacho
{
    /// <summary>
    /// PTD_CD_MERC_DST - Código Mercadoria Destino
    /// </summary>
    public string PtdCdMercDst { get; set; }

    /// <summary>
    /// PTD_DTC_SEL_FTRM - Timestamp Seleção Faturamento
    /// </summary>
    public DateTime PtdDtcSelFtrm { get; set; }

    /// <summary>
    /// PTD_LGON_FUNC - Login Funcionário
    /// </summary>
    public string PtdLgonFunc { get; set; } = string.Empty;

    /// <summary>
    /// PTD_CD_SEQ - Código Sequência
    /// </summary>
    public int PtdCdSeq { get; set; }

    /// <summary>
    /// PTD_CD_T_REC - Código Tipo Recolhimento
    /// </summary>
    public int? PtdCdTRec { get; set; }

    /// <summary>
    /// PTD_CD_T_MTZ - Código Tipo Motorização
    /// </summary>
    public int? PtdCdTMtz { get; set; }

    /// <summary>
    /// PTD_ID_MTZ - Identificador Motorização
    /// </summary>
    public int? PtdIdMtz { get; set; }

    /// <summary>
    /// PTD_ID_CLI - Identificador Cliente
    /// </summary>
    public int? PtdIdCli { get; set; }

    /// <summary>
    /// PTD_ID_PTC_DSP - Identificador Protocolo Despacho
    /// </summary>
    public string PtdIdPtcDsp { get; set; } = string.Empty;

    /// <summary>
    /// PTD_DTC_PTC_DSP - Data/Hora Protocolo Despacho
    /// </summary>
    public DateTime? PtdDtcPtcDsp { get; set; }

    /// <summary>
    /// PTD_CD_PGT_FRT - Código Pagamento Frete
    /// </summary>
    public string? PtdCdPgtFrt { get; set; }

    /// <summary>
    /// PTD_CD_TRSR - Código Transportadora
    /// </summary>
    public string? PtdCdTrsr { get; set; }

    /// <summary>
    /// PTD_PESO_TTL_LQD - Peso Total Líquido (em gramas)
    /// </summary>
    public decimal? PtdPesoTtlLqd { get; set; }

    /// <summary>
    /// PTD_PESO_TTL_BRT - Peso Total Bruto (em gramas)
    /// </summary>
    public decimal? PtdPesoTtlBrt { get; set; }

    /// <summary>
    /// PTD_Q_TTL_VOL - Quantidade Total Volumes
    /// </summary>
    public int? PtdQTtlVol { get; set; }

    /// <summary>
    /// PTD_V_TTL_MRCD - Valor Total Mercadoria (em centavos)
    /// </summary>
    public decimal? PtdVTtlMrcd { get; set; }

    /// <summary>
    /// PTD_IC_DSP_IMPS - Indicador Despacho Impresso
    /// </summary>
    public string PtdIcDspImps { get; set; } = "N";
}
