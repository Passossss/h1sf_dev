namespace H1SF.Domain.Entities.Faturamento;

/// <summary>
/// WS36-ARE-RECEIVE - Área de recebimento de parâmetros (24 bytes)
/// </summary>
public class FaturamentoParametros
{
    // WS36-CD-MERC-DST - PIC X(001)
    public required char CodigoMercadoriaDestino { get; set; }
    
    // WS36-DTC-SEL-FTRM - PIC X(014)
    public required string DataHoraSelecao { get; set; } = string.Empty;
    
    // WS36-LGON-FUNC - PIC X(008)
    public required string LoginFuncionario { get; set; } = string.Empty;
    
    // WS36-FASE-FTRM - PIC X(001)
    public required char FaseFaturamento { get; set; }
}
