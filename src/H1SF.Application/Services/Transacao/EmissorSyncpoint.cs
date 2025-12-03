using H1SF.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Transacao;

/// <summary>
/// 590-00-EMITE-SYNCPOINT - Emite SYNCPOINT (commit de transação)
/// Linhas COBOL: aproximadamente 6000-6010
/// Autor: A.C.ANDREATTA
/// </summary>
public class EmissorSyncpoint : IEmissorSyncpoint
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmissorSyncpoint> _logger;

    public EmissorSyncpoint(
        ApplicationDbContext context,
        ILogger<EmissorSyncpoint> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 590-00-EMITE-SYNCPOINT SECTION
    /// EXEC CICS SYNCPOINT END-EXEC
    /// 
    /// No ambiente CICS mainframe, SYNCPOINT:
    /// - Confirma (commit) todas as alterações no banco de dados
    /// - Libera todos os locks de recursos
    /// - Marca um ponto de consistência na transação
    /// 
    /// Em .NET/EF Core, isso equivale a:
    /// - SaveChangesAsync() - persiste alterações pendentes
    /// - Commit de transação (se houver uma transaction scope ativa)
    /// </summary>
    public async Task EmitirSyncpointAsync()
    {
        _logger.LogDebug("590-00-EMITE-SYNCPOINT iniciado");

        try
        {
            // Verifica se há alterações pendentes para commitar
            var changesPending = _context.ChangeTracker.HasChanges();

            if (changesPending)
            {
                _logger.LogInformation("SYNCPOINT: Confirmando alterações pendentes no banco de dados");
                
                // Equivalente ao SYNCPOINT do CICS - commit de todas as alterações
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("SYNCPOINT: {Quantidade} alteração(ões) confirmada(s) com sucesso", 
                    _context.ChangeTracker.Entries().Count(e => e.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged));
            }
            else
            {
                _logger.LogDebug("SYNCPOINT: Nenhuma alteração pendente para confirmar");
            }

            _logger.LogDebug("590-00-EMITE-SYNCPOINT concluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao emitir SYNCPOINT");
            throw;
        }
    }
}
