using H1SF.Application.Services.Recolhimento;
using H1SF.Infrastructure.Repositories.Recolhimento;
using Microsoft.AspNetCore.Mvc;

namespace H1SF.API.Controllers;

/// <summary>
/// Controller para 537-00-FINALIZA-ITEM-REC-PEND
/// Finaliza itens de recolhimento pendentes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemRecolhimentoController : ControllerBase
{
    private readonly IFinalizadorItemRecolhimento _finalizador;
    private readonly ILogger<ItemRecolhimentoController> _logger;

    public ItemRecolhimentoController(
        IFinalizadorItemRecolhimento finalizador,
        ILogger<ItemRecolhimentoController> logger)
    {
        _finalizador = finalizador;
        _logger = logger;
    }

    /// <summary>
    /// 537-00-FINALIZA-ITEM-REC-PEND
    /// Finaliza itens de recolhimento que estão pendentes
    /// </summary>
    /// <returns>Resultado da finalização</returns>
    /// <remarks>
    /// Esta operação busca itens de recolhimento que:
    /// - Já foram finalizados no recolhimento (DTC_FNL_REC preenchido)
    /// - Ainda não foram finalizados como item (DTC_FNL_ITEM nulo)
    /// - Estão associados a faturamentos concluídos (MNT_DTC_FNL_FTRM preenchido)
    /// 
    /// E atualiza DTC_FNL_ITEM com a data de finalização do faturamento correspondente.
    /// 
    /// Processo COBOL equivalente:
    /// 1. SELECT para verificar existência de itens pendentes
    /// 2. Se não encontrar, retorna (GO TO EXIT)
    /// 3. Se encontrar, executa UPDATE correlacionado com subquery
    /// </remarks>
    [HttpPost("finalizar-pendentes")]
    [ProducesResponseType(typeof(FinalizarItemRecolhimentoOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FinalizarItemRecolhimentoOutputDto>> FinalizarPendentes()
    {
        try
        {
            _logger.LogInformation("Iniciando finalização de itens de recolhimento pendentes");

            var resultado = await _finalizador.FinalizarItensPendentesAsync();

            if (resultado.ExistiamItensPendentes)
            {
                _logger.LogInformation(
                    "Finalização concluída: {Quantidade} item(ns) finalizado(s)",
                    resultado.QuantidadeItensFinalizados);
            }
            else
            {
                _logger.LogInformation("Nenhum item pendente encontrado");
            }

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao finalizar itens de recolhimento pendentes");
            return StatusCode(500, new 
            { 
                Erro = "Erro interno ao processar requisição",
                Detalhes = ex.Message
            });
        }
    }

    /// <summary>
    /// Verifica se existem itens pendentes sem executar a finalização
    /// </summary>
    /// <returns>True se existem itens pendentes</returns>
    [HttpGet("verificar-pendentes")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> VerificarPendentes(
        [FromServices] IFinalizadorItemRecolhimentoRepository repository)
    {
        try
        {
            var existem = await repository.ExistemItensPendentesAsync();
            
            _logger.LogInformation(
                "Verificação de pendentes: {Resultado}",
                existem ? "Existem itens pendentes" : "Nenhum item pendente");

            return Ok(new 
            { 
                ExistemPendentes = existem,
                Mensagem = existem 
                    ? "Existem itens de recolhimento pendentes de finalização" 
                    : "Nenhum item pendente encontrado"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar itens pendentes");
            return StatusCode(500, new { Erro = "Erro interno ao processar requisição" });
        }
    }
}
