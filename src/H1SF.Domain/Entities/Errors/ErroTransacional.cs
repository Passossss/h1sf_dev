namespace H1SF.Domain.Entities.Errors;

/// <summary>
/// ACB5010-MSG-ERRO - Estrutura de erro transacional
/// Equivalente aos campos de erro do COBOL
/// </summary>
public class ErroTransacional
{
    // WS35-ID-PGM
    public string Programa { get; set; } = string.Empty;
    
    // WS35-MSG-ERRO-LIT + WS35-MSG-ERRO-VAR
    public string Mensagem { get; set; } = string.Empty;
    
    // Dados originais recebidos
    public string DadosOriginais { get; set; } = string.Empty;
    
    public DateTime DataHora { get; set; }
    
    public string Severidade { get; set; } = string.Empty;
}
