using H1SF.Application.Services.Transacao;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para 625-00-START-SF30
/// Inicia transação SF30 (CICS START)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransacaoSf30Controller : ControllerBase
{
    private readonly IIniciadorTransacaoSf30 _iniciadorTransacao;
    private readonly ILogger<TransacaoSf30Controller> _logger;

    public TransacaoSf30Controller(
        IIniciadorTransacaoSf30 iniciadorTransacao,
        ILogger<TransacaoSf30Controller> logger)
    {
        _iniciadorTransacao = iniciadorTransacao;
        _logger = logger;
    }

    /// <summary>
    /// 625-00-START-SF30
    /// Inicia transação SF30 de forma assíncrona
    /// </summary>
    /// <param name="input">Dados da transação</param>
    /// <returns>Confirmação de início</returns>
    [HttpPost("iniciar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> IniciarTransacao([FromBody] IniciarTransacaoSf30InputDto input)
    {
        try
        {
            _logger.LogInformation("Iniciando transação SF30");

            await _iniciadorTransacao.IniciarTransacaoSf30Async(input);

            _logger.LogInformation("Transação SF30 iniciada com sucesso");

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Transação SF30 iniciada com sucesso",
                TransactionId = input.TransactionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar transação SF30");
            return StatusCode(500, new { Erro = "Erro interno ao processar requisição" });
        }
    }
}
