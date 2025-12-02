namespace H1SF.Domain.Entities.Faturamento;

/// <summary>
/// Representa H1SF.MNT_MONITOR_FTRM
/// </summary>
public class MonitorFaturamento
{
    // Chave primária
    public required char CodigoMercadoriaDestino { get; set; }
    public required string TimestampSelecao { get; set; }
    public required string LoginFuncionario { get; set; }
    
    // Campos
    public char? MotivoInterrupcao { get; set; }        // MNT_ID_MTV_ITRP
    public char? IndicadorCancelamento { get; set; }     // MNT_IC_CAN
    public DateTime? DataFaseMontagem { get; set; }      // MNT_DTC_FASE_MTG
    public DateTime? DataFaseComplementar { get; set; }  // MNT_DTC_FASE_MTG_CPLM
}
