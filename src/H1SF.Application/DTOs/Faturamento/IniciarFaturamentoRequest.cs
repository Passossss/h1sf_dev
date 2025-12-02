namespace H1SF.API.DTOs;

/// <summary>
/// Request DTO - Equivalente aos 24 bytes do WS36-ARE-RECEIVE do COBOL
/// </summary>
public class IniciarFaturamentoRequest
{
    /// <summary>
    /// WS36-CD-MERC-DST - PIC X(001) - 'D' (Doméstico) ou 'E' (Exportação)
    /// </summary>
    public required char CodigoMercadoriaDestino { get; set; }
    
    /// <summary>
    /// WS36-DTC-SEL-FTRM - PIC X(014) - Formato: YYYYMMDDHHmmss
    /// </summary>
    public required string TimestampSelecao { get; set; }
    
    /// <summary>
    /// WS36-LGON-FUNC - PIC X(008) - Login do funcionário
    /// </summary>
    public required string LoginFuncionario { get; set; }
    
    /// <summary>
    /// WS36-FASE-FTRM - PIC X(001) - '1' (Montagem) ou '2' (Complementar)
    /// </summary>
    public required char FaseFaturamento { get; set; }
}
