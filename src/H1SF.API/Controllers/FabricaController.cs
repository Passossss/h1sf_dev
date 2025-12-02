using H1SF.Application.Services.Fabrica;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// 572-00-SQL-RECUPERA-CNPJ - Controller para recuperação de CNPJ da fábrica
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FabricaController : ControllerBase
{
    private readonly IRecuperadorCnpjFabrica _recuperadorCnpj;
    private readonly ILogger<FabricaController> _logger;

    public FabricaController(
        IRecuperadorCnpjFabrica recuperadorCnpj,
        ILogger<FabricaController> logger)
    {
        _recuperadorCnpj = recuperadorCnpj;
        _logger = logger;
    }

    /// <summary>
    /// Recupera CNPJ da fábrica
    /// POST /api/fabrica/recuperar-cnpj
    /// </summary>
    [HttpPost("recuperar-cnpj")]
    public async Task<IActionResult> RecuperarCnpj([FromBody] RecuperarCnpjRequest request)
    {
        try
        {
            var resultado = await _recuperadorCnpj.RecuperarCnpjAsync(
                request.CdMercDst,
                request.FaseFtrm,
                request.CdMercDstInt,
                request.DtcSelFtrm,
                request.LgonFunc,
                request.IcSim);

            if (resultado == null)
            {
                return NotFound(new 
                { 
                    Erro = "CNPJ da fábrica não encontrado",
                    Request = request
                });
            }

            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Erro ao recuperar CNPJ");
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao recuperar CNPJ");
            return StatusCode(500, new { Erro = "Erro interno do servidor" });
        }
    }
}

/// <summary>
/// Request para recuperação de CNPJ
/// </summary>
public record RecuperarCnpjRequest(
    string CdMercDst,
    string FaseFtrm,
    int CdMercDstInt,
    DateTime DtcSelFtrm,
    string LgonFunc,
    string IcSim
);
