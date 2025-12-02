namespace H1SF.Application.DTOs.DataHora;

/// <summary>
/// DTO para retorno de data/hora do sistema (COBOL: CB0001-RRE-DTC-GRC)
/// Formato: YYYYMMDDHH24MISS (14 caracteres)
/// </summary>
public class DataHoraSistemaDto
{
    /// <summary>
    /// Data e hora no formato YYYYMMDDHH24MISS
    /// Exemplo: "20231215143025" = 15/12/2023 14:30:25
    /// COBOL: CB0001-RRE-DTC-GRC PIC X(014)
    /// </summary>
    public string DataHoraFormatada { get; set; } = string.Empty;
    
    /// <summary>
    /// Data/hora como DateTime para uso interno
    /// </summary>
    public DateTime DataHora { get; set; }
}
