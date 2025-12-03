using H1SF.Application.Services.Monitor;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para 565-00-ATUALIZA-LBRC-IMPS
/// Atualiza fase LBRC_IMPS no monitor de faturamento
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FaseLbrcImpsController : ControllerBase
{
    private readonly IAtualizadorFaseLbrcImps _atualizadorFase;
    private readonly ILogger<FaseLbrcImpsController> _logger;

    public FaseLbrcImpsController(
        IAtualizadorFaseLbrcImps atualizadorFase,
        ILogger<FaseLbrcImpsController> logger)
    {
        _atualizadorFase = atualizadorFase;
        _logger = logger;
    }

    /// <summary>
    /// 565-00-ATUALIZA-LBRC-IMPS
    /// Atualiza timestamp da fase LBRC_IMPS no monitor de faturamento
    /// </summary>
    /// <param name="input">Parâmetros da seleção de faturamento</param>
    /// <returns>Confirmação de atualização</returns>
    /// <remarks>
    /// LBRC_IMPS (Liberação/Conclusão de Impressão) é uma fase do processo de faturamento
    /// que indica quando a impressão de documentos foi liberada ou concluída.
    /// 
    /// Esta operação atualiza o campo MNT_DTC_FASE_LBRC_IMPS com o timestamp atual (SYSDATE).
    /// </remarks>
    [HttpPut("atualizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AtualizarFase([FromBody] AtualizarFaseLbrcImpsInputDto input)
    {
        try
        {
            _logger.LogInformation(
                "Atualizando fase LBRC_IMPS - Mercado: {Mercado}, Data: {Data}",
                input.CodigoMercadoDestino,
                input.DataSelecaoFaturamento);

            await _atualizadorFase.AtualizarFaseLbrcImpsAsync(input);

            _logger.LogInformation("Fase LBRC_IMPS atualizada com sucesso");

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Fase LBRC_IMPS atualizada com sucesso",
                DataHoraAtualizacao = DateTime.Now
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar fase LBRC_IMPS");
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar fase LBRC_IMPS");
            return StatusCode(500, new { Erro = "Erro interno ao processar requisição" });
        }
    }

    /// <summary>
    /// Endpoint alternativo usando query string (para compatibilidade)
    /// </summary>
    [HttpPut("atualizar/{mercado}/{data}/{usuario}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AtualizarFasePorParametros(
        string mercado,
        string data,
        string usuario)
    {
        var input = new AtualizarFaseLbrcImpsInputDto
        {
            CodigoMercadoDestino = mercado,
            DataSelecaoFaturamento = data,
            LoginFuncionario = usuario
        };

        return await AtualizarFase(input);
    }
}
