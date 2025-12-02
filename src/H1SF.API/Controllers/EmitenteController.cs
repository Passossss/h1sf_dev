using H1SF.Application.Services.Emitente;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// 505-00-RECUPERA-EMITENTE - Controller para recuperação de dados do emitente
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmitenteController : ControllerBase
{
    private readonly IRecuperadorEmitente _recuperadorEmitente;
    private readonly ILogger<EmitenteController> _logger;

    public EmitenteController(
        IRecuperadorEmitente recuperadorEmitente,
        ILogger<EmitenteController> logger)
    {
        _recuperadorEmitente = recuperadorEmitente;
        _logger = logger;
    }

    /// <summary>
    /// Recupera dados completos do emitente
    /// GET /api/emitente/{cnpj}
    /// </summary>
    [HttpGet("{cnpj}")]
    public async Task<IActionResult> RecuperarEmitente(string cnpj)
    {
        try
        {
            var resultado = await _recuperadorEmitente.RecuperarEmitenteAsync(cnpj);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Emitente não encontrado");
            return NotFound(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao recuperar emitente");
            return StatusCode(500, new { Erro = "Erro interno do servidor" });
        }
    }
}
