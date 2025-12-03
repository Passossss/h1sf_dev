namespace H1SF.Application.Services.Transacao;

/// <summary>
/// Interface para 625-00-START-SF30
/// Inicia transação SF30 (CICS START TRANSID)
/// </summary>
public interface IIniciadorTransacaoSf30
{
    /// <summary>
    /// 625-00-START-SF30 SECTION
    /// EXEC CICS START TRANSID ('SF30') FROM (WS36-ARE-SF30)
    /// </summary>
    Task IniciarTransacaoSf30Async(IniciarTransacaoSf30InputDto input);
}

/// <summary>
/// Input para iniciar transação SF30
/// </summary>
public class IniciarTransacaoSf30InputDto
{
    /// <summary>
    /// WS36-ARE-SF30 - Área de dados para transação SF30
    /// Contém os dados necessários para processar a transação
    /// </summary>
    public string AreaDadosSf30 { get; set; } = string.Empty;
    
    /// <summary>
    /// Identificador da transação (normalmente 'SF30')
    /// </summary>
    public string TransactionId { get; set; } = "SF30";
}
