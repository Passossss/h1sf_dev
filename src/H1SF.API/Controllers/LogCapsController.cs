using H1SF.Application.Services.LogCaps;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para 875-00-MONTA-LOG-CAPS
/// Gera log de sincronização CAPS (interface MQ)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LogCapsController : ControllerBase
{
    private readonly IMontadorLogCaps _montadorLogCaps;
    private readonly ILogger<LogCapsController> _logger;

    public LogCapsController(
        IMontadorLogCaps montadorLogCaps,
        ILogger<LogCapsController> logger)
    {
        _montadorLogCaps = montadorLogCaps;
        _logger = logger;
    }

    /// <summary>
    /// 875-00-MONTA-LOG-CAPS
    /// Monta log de sincronização CAPS e grava em fila MQ
    /// </summary>
    /// <param name="input">Parâmetros da seleção de faturamento</param>
    /// <returns>Confirmação de processamento</returns>
    [HttpPost("montar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> MontarLogCaps([FromBody] MontarLogCapsInputDto input)
    {
        try
        {
            _logger.LogInformation(
                "Iniciando montagem de log CAPS - Mercado: {Mercado}, Data: {Data}",
                input.CodigoMercadoDestino,
                input.DataSelecaoFaturamento);

            await _montadorLogCaps.MontarLogCapsAsync(input);

            _logger.LogInformation("Log CAPS montado com sucesso");

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Log CAPS gerado com sucesso"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao montar log CAPS");
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao montar log CAPS");
            return StatusCode(500, new { Erro = "Erro interno ao processar requisição" });
        }
    }
}
