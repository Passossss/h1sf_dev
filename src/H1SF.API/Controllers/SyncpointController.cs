using H1SF.Application.Services.Transacao;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para operações de transação (SYNCPOINT)
/// 590-00-EMITE-SYNCPOINT
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SyncpointController : ControllerBase
{
    private readonly IEmissorSyncpoint _emissorSyncpoint;
    private readonly ILogger<SyncpointController> _logger;

    public SyncpointController(
        IEmissorSyncpoint emissorSyncpoint,
        ILogger<SyncpointController> logger)
    {
        _emissorSyncpoint = emissorSyncpoint;
        _logger = logger;
    }

    /// <summary>
    /// 590-00-EMITE-SYNCPOINT
    /// Confirma (commit) todas as alterações pendentes no banco de dados
    /// Equivalente ao EXEC CICS SYNCPOINT END-EXEC
    /// </summary>
    /// <returns>Confirmação do commit</returns>
    [HttpPost]
    public async Task<IActionResult> EmitirSyncpoint()
    {
        try
        {
            _logger.LogInformation("Emitindo SYNCPOINT");
            
            await _emissorSyncpoint.EmitirSyncpointAsync();
            
            return Ok(new { 
                mensagem = "SYNCPOINT emitido com sucesso",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao emitir SYNCPOINT");
            return StatusCode(500, new { 
                erro = "Erro ao emitir SYNCPOINT",
                detalhe = ex.Message
            });
        }
    }
}
