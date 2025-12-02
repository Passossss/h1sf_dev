using H1SF.Application.Services.FaturamentoPws;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers
{
    [ApiController]
    [Route("api/faturamento/atualizarpws")]
    public class AtualizarPwsController : ControllerBase
    {
        private readonly IAtualizarPwsService _atualizarPwsService;
        private readonly ILogger<AtualizarPwsController> _logger;

        public AtualizarPwsController(
            IAtualizarPwsService atualizarPwsService,
            ILogger<AtualizarPwsController> logger)
        {
            _atualizarPwsService = atualizarPwsService;
            _logger = logger;
        }

        [HttpPost("executar")]
        public async Task<ActionResult<AtualizarPwsOutputDto>> ExecutarAtualizacao(
            [FromBody] AtualizarPwsInputDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _atualizarPwsService.ExecutarAtualizacaoPwsAsync(input);

                if (!resultado.Sucesso)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint de atualização PWS");

                return StatusCode(500, new AtualizarPwsOutputDto
                {
                    Sucesso = false,
                    Mensagem = $"Erro interno: {ex.Message}",
                    DataExecucao = DateTime.Now
                });
            }
        }
    }
}
