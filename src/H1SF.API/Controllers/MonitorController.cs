using H1SF.Application.Services;
using H1SF.Domain.Entities.Faturamento;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MonitorController : ControllerBase
{
    private readonly IAtualizadorMonitor _atualizadorMonitor;
    private readonly ILogger<MonitorController> _logger;

    public MonitorController(
        IAtualizadorMonitor atualizadorMonitor,
        ILogger<MonitorController> logger)
    {
        _atualizadorMonitor = atualizadorMonitor;
        _logger = logger;
    }

    /// <summary>
    /// Atualiza o monitor de faturamento (560-00-ATUALIZA-MONITOR)
    /// </summary>
    [HttpPost("atualizar")]
    public async Task<IActionResult> AtualizarMonitor([FromBody] AtualizarMonitorRequest request)
    {
        try
        {
            _logger.LogInformation("Atualizando monitor para {CodigoMercadoriaDestino}/{DataHoraSelecao}/{LoginFuncionario}",
                request.CodigoMercadoriaDestino,
                request.DataHoraSelecao,
                request.LoginFuncionario);

            var parametros = new FaturamentoParametros
            {
                CodigoMercadoriaDestino = request.CodigoMercadoriaDestino,
                DataHoraSelecao = request.DataHoraSelecao,
                LoginFuncionario = request.LoginFuncionario,
                FaseFaturamento = request.FaseFaturamento
            };

            await _atualizadorMonitor.AtualizarMonitorAsync(parametros);

            return Ok(new
            {
                Sucesso = true,
                FoiCancelado = _atualizadorMonitor.FoiCancelado,
                Mensagem = _atualizadorMonitor.FoiCancelado 
                    ? "Monitor atualizado - Faturamento cancelado" 
                    : "Monitor atualizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar monitor");
            return BadRequest(new { Sucesso = false, Erro = ex.Message });
        }
    }

    /// <summary>
    /// Verifica se o faturamento foi cancelado
    /// </summary>
    [HttpGet("cancelado")]
    public IActionResult VerificarCancelamento()
    {
        return Ok(new
        {
            FoiCancelado = _atualizadorMonitor.FoiCancelado
        });
    }
}

public class AtualizarMonitorRequest
{
    public char CodigoMercadoriaDestino { get; set; }
    public string DataHoraSelecao { get; set; } = string.Empty;
    public string LoginFuncionario { get; set; } = string.Empty;
    public char FaseFaturamento { get; set; }
}
