namespace H1SF.Application.Services.Transacao;

/// <summary>
/// Interface para 645-00-START-SF31
/// Inicia transação SF31 (CICS START TRANSID)
/// </summary>
public interface IIniciadorTransacaoSf31
{
    /// <summary>
    /// 645-00-START-SF31 SECTION
    /// EXEC CICS START TRANSID ('SF31') FROM (WS36-ARE-SF31)
    /// </summary>
    Task IniciarTransacaoSf31Async(IniciarTransacaoSf31InputDto input);
}

/// <summary>
/// Input para iniciar transação SF31
/// </summary>
public class IniciarTransacaoSf31InputDto
{
    /// <summary>
    /// WS36-ARE-SF31 - Área de dados para transação SF31
    /// Contém os dados necessários para processar a transação
    /// </summary>
    public string AreaDadosSf31 { get; set; } = string.Empty;
    
    /// <summary>
    /// Identificador da transação (normalmente 'SF31')
    /// </summary>
    public string TransactionId { get; set; } = "SF31";
}
