using H1SF.Application.Services.DataHora;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataHoraController : ControllerBase
{
    private readonly IRecuperadorDataHora _recuperadorDataHora;
    private readonly ILogger<DataHoraController> _logger;

    public DataHoraController(
        IRecuperadorDataHora recuperadorDataHora,
        ILogger<DataHoraController> logger)
    {
        _recuperadorDataHora = recuperadorDataHora;
        _logger = logger;
    }

    /// <summary>
    /// Recupera data/hora do sistema (510-00-RECUPERA-DATA-HORA)
    /// </summary>
    /// <returns>Data/hora no formato YYYYMMDDHH24MISS</returns>
    [HttpGet("sistema")]
    public async Task<IActionResult> RecuperarDataHoraSistema()
    {
        try
        {
            _logger.LogInformation("Recuperando data/hora do sistema");

            var resultado = await _recuperadorDataHora.RecuperarDataHoraAsync();

            return Ok(new
            {
                Sucesso = true,
                DataHoraFormatada = resultado.DataHoraFormatada,
                DataHora = resultado.DataHora,
                Mensagem = "Data/hora recuperada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar data/hora do sistema");
            return BadRequest(new { Sucesso = false, Erro = ex.Message });
        }
    }
}
