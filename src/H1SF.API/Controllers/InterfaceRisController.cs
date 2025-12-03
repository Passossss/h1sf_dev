using H1SF.Application.DTOs.EntradaNfIcRis;
using H1SF.Application.Services.EntradaNfIcRis;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers
{
    [ApiController]
    [Route("api/interface-ris")]
    public class InterfaceRisController : ControllerBase
    {
        private readonly IEntradaRisService _service;
        private readonly ILogger<InterfaceRisController> _logger;

        public InterfaceRisController(
            IEntradaRisService service,
            ILogger<InterfaceRisController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("enviar-entrada-nf")]
        public async Task<ActionResult<EnviarInterfaceRisOutputDto>> EnviarEntradaNf(
            [FromBody] EnviarInterfaceRisInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _logger.LogInformation(
                    "Recebida requisição entrada NF IC RIS. Mercador: {CdMercDst}, Data: {DtcSelFtrm}",
                    input.CdMercDst, input.DtcSelFtrm);

                var resultado = await _service.ExecutarEntradaNfIcRisAsync(input);

                if (!resultado.Sucesso)
                {
                    _logger.LogWarning(
                        "Falha na interface RIS. ECI: {CdRetrEci}, ACES: {CdRetrAces}",
                        resultado.CdRetrEci, resultado.CdRetrAces);

                    return BadRequest(resultado);
                }

                _logger.LogInformation("Interface RIS processada com sucesso. ID: {IdRequisicao}", resultado.IdRequisicao);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint de interface RIS");

                return StatusCode(500, new EnviarInterfaceRisOutputDto
                {
                    Sucesso = false,
                    Mensagem = $"Erro interno: {ex.Message}",
                    DataExecucao = DateTime.Now
                });
            }
        }

        [HttpPost("enviar-interface")]
        public async Task<ActionResult<EnviarInterfaceRisOutputDto>> EnviarInterface(
            [FromBody] EnviarInterfaceRisInputDto input)
        {
            // Endpoint alternativo
            return await EnviarEntradaNf(input);
        }
    }
}
