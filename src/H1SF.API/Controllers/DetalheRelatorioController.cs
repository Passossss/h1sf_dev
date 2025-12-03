using H1SF.Application.DTOs.DreDetalhesRelatorio;
using H1SF.Application.Services.DreDetalhesRelatorio;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers
{
    ApiController]
[Route("api/dre-detalhes-relatorio")]
    public class DetalheRelatorioController : ControllerBase
    {
        private readonly IDetalheRelatorioService _service;
        private readonly ILogger<DetalheRelatorioController> _logger;

        public DetalheRelatorioController(
            IDetalheRelatorioService service,
            ILogger<DetalheRelatorioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("inserir-detalhe")]
        public async Task<ActionResult<InserirDetalheOutputDto>> InserirDetalhe(
            [FromBody] InserirDetalheInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _service.ExecutarInsercaoDetalheAsync(input);

                if (!resultado.Sucesso)
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint de inserção de detalhe");

                return StatusCode(500, new InserirDetalheOutputDto
                {
                    Sucesso = false,
                    Mensagem = $"Erro interno: {ex.Message}",
                    DataExecucao = DateTime.Now
                });
            }
        }
    }
}
