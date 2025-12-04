using H1SF.Application.Services.Memoria;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para operações de gerenciamento de memória
/// 615-00-FREEMAIN-TRSC
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MemoriaController : ControllerBase
{
    private readonly ILiberadorMemoria _liberadorMemoria;
    private readonly ILogger<MemoriaController> _logger;

    public MemoriaController(
        ILiberadorMemoria liberadorMemoria,
        ILogger<MemoriaController> logger)
    {
        _liberadorMemoria = liberadorMemoria;
        _logger = logger;
    }

    /// <summary>
    /// 615-00-FREEMAIN-TRSC
    /// Libera memória alocada dinamicamente (equivalente ao CICS FREEMAIN)
    /// </summary>
    /// <param name="chaveGetmain">Chave indicando se houve GETMAIN anterior ('S' ou 'N')</param>
    /// <returns>Confirmação da liberação</returns>
    [HttpPost("liberar")]
    public async Task<IActionResult> LiberarMemoria([FromQuery] string chaveGetmain = "N")
    {
        try
        {
            _logger.LogInformation("Executando FREEMAIN com chave: {ChaveGetmain}", chaveGetmain);
            
            await _liberadorMemoria.LiberarMemoriaAsync(chaveGetmain);
            
            return Ok(new   
            { 
                mensagem = chaveGetmain?.Equals("S", StringComparison.OrdinalIgnoreCase) == true 
                    ? "FREEMAIN executado - memória liberada" 
                    : "FREEMAIN não executado - chave GETMAIN inválida",
                chaveGetmain = chaveGetmain,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar FREEMAIN");
            return StatusCode(500, new 
            { 
                erro = "Erro ao liberar memória",
                detalhe = ex.Message
            });
        }
    }

    /// <summary>
    /// Obtém informações sobre o uso de memória atual
    /// </summary>
    [HttpGet("info")]
    public IActionResult ObterInfoMemoria()
    {
        try
        {
            var memoriaAlocada = GC.GetTotalMemory(forceFullCollection: false);
            var geracoes = new
            {
                geracao0 = GC.CollectionCount(0),
                geracao1 = GC.CollectionCount(1),
                geracao2 = GC.CollectionCount(2)
            };

            return Ok(new
            {
                memoriaAlocadaBytes = memoriaAlocada,
                memoriaAlocadaMB = Math.Round(memoriaAlocada / 1024.0 / 1024.0, 2),
                colecoesGC = geracoes,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter informações de memória");
            return StatusCode(500, new
            {
                erro = "Erro ao obter informações de memória",
                detalhe = ex.Message
            });
        }
    }
}
