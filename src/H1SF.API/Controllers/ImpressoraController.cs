using H1SF.Application.DTO;
using H1SF.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImpressoraController : ControllerBase
{
    private readonly IImpressoraService _impressoraService;
    private readonly ILogger<ImpressoraController> _logger;

    public ImpressoraController(
        IImpressoraService impressoraService,
        ILogger<ImpressoraController> logger)
    {
        _impressoraService = impressoraService;
        _logger = logger;
    }

    /// <summary>
    /// Define a impressora para um faturamento
    /// </summary>
    [HttpPost("definir")]
    public async Task<ActionResult<DefinirImpressoraOutputDto>> DefinirImpressora(
        [FromBody] DefinirImpressoraInputDto input)
    {
        _logger.LogInformation("Recebida requisição para definir impressora");

        var resultado = await _impressoraService.DefinirImpressoraAsync(input);

        if (!resultado.Sucesso)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    /// <summary>
    /// Obtém o tipo de recolhimento
    /// </summary>
    [HttpGet("tipo-recolhimento")]
    public async Task<ActionResult<int?>> ObterTipoRecolhimento(
        [FromQuery] int cdMercDst,
        [FromQuery] DateTime dtcSelFtrm,
        [FromQuery] string lgonFunc)
    {
        _logger.LogInformation("Obtendo tipo de recolhimento para CD_MERC_DST: {CdMercDst}", cdMercDst);

        var resultado = await _impressoraService.ObterTipoRecolhimentoAsync(
            cdMercDst, 
            dtcSelFtrm, 
            lgonFunc);

        if (resultado == null)
        {
            return NotFound();
        }

        return Ok(resultado);
    }
}
