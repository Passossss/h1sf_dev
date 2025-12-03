namespace H1SF.Application.Services.Transacao;

/// <summary>
/// Interface para 590-00-EMITE-SYNCPOINT
/// Emite SYNCPOINT (commit de transação CICS)
/// </summary>
public interface IEmissorSyncpoint
{
    /// <summary>
    /// 590-00-EMITE-SYNCPOINT SECTION
    /// EXEC CICS SYNCPOINT END-EXEC
    /// 
    /// No CICS, SYNCPOINT confirma (commit) todas as alterações de banco de dados
    /// e libera locks desde o último SYNCPOINT ou início da transação.
    /// </summary>
    Task EmitirSyncpointAsync();
}
