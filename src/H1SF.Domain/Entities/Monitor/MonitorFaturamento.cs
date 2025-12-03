namespace H1SF.Domain.Entities.Faturamento;

/// <summary>
/// Representa H1SF.MNT_MONITOR_FTRM
/// </summary>
public class MonitorFaturamento
{
    // Chave primária
    public required char CodigoMercadoriaDestino { get; set; }  // MNT_CD_MERC_DST
    public required string TimestampSelecao { get; set; }       // MNT_DTC_SEL_FTRM
    public required string LoginFuncionario { get; set; }       // MNT_LGON_FUNC
    
    // Campos
    public int? CodigoTipoRecolhimento { get; set; }     // MNT_CD_T_REC
    public char? MotivoInterrupcao { get; set; }        // MNT_ID_MTV_ITRP
    public char? IndicadorCancelamento { get; set; }     // MNT_IC_CAN
    public DateTime? DataFaseMontagem { get; set; }      // MNT_DTC_FASE_MTG
    public DateTime? DataFaseComplementar { get; set; }  // MNT_DTC_FASE_MTG_CPLM
    public DateTime? DataFaseLbrcImps { get; set; }      // MNT_DTC_FASE_LBRC_IMPS
    public DateTime? DataFinalizacaoFaturamento { get; set; } // MNT_DTC_FNL_FTRM
}
