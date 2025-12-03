using H1SF.Infrastructure.Repositories.Recolhimento;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Recolhimento;

/// <summary>
/// 537-00-FINALIZA-ITEM-REC-PEND - Finaliza itens de recolhimento pendentes
/// Linhas COBOL: aproximadamente 4500-4600
/// Autor: E. FRIOLI JR.
/// </summary>
public class FinalizadorItemRecolhimento : IFinalizadorItemRecolhimento
{
    private readonly IFinalizadorItemRecolhimentoRepository _repository;
    private readonly ILogger<FinalizadorItemRecolhimento> _logger;

    public FinalizadorItemRecolhimento(
        IFinalizadorItemRecolhimentoRepository repository,
        ILogger<FinalizadorItemRecolhimento> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 537-00-FINALIZA-ITEM-REC-PEND SECTION
    /// 
    /// Lógica COBOL:
    /// 1. Verifica se existem itens de recolhimento com:
    ///    - DTC_FNL_REC IS NOT NULL (recolhimento finalizado)
    ///    - DTC_FNL_ITEM IS NULL (item ainda não finalizado)
    ///    - Que estão associados a faturamentos concluídos (MNT_DTC_FNL_FTRM IS NOT NULL)
    /// 
    /// 2. Se não encontrar, retorna (GO TO 537-99-SQL-FINALIZA-EXIT)
    /// 
    /// 3. Se encontrar, atualiza DTC_FNL_ITEM com a data de finalização do faturamento
    /// 
    /// Esta rotina garante que itens de recolhimento sejam marcados como finalizados
    /// quando o processo de faturamento associado for concluído.
    /// </summary>
    public async Task<FinalizarItemRecolhimentoOutputDto> FinalizarItensPendentesAsync()
    {
        _logger.LogDebug("537-00-FINALIZA-ITEM-REC-PEND iniciado");

        try
        {
            // Verifica se existem itens pendentes
            var existemPendentes = await _repository.ExistemItensPendentesAsync();

            if (!existemPendentes)
            {
                _logger.LogInformation("Nenhum item de recolhimento pendente encontrado");
                
                return new FinalizarItemRecolhimentoOutputDto
                {
                    ExistiamItensPendentes = false,
                    QuantidadeItensFinalizados = 0,
                    Mensagem = "Nenhum item de recolhimento pendente encontrado"
                };
            }

            _logger.LogInformation("Itens de recolhimento pendentes encontrados. Iniciando finalização...");

            // Finaliza os itens pendentes
            var quantidadeFinalizados = await _repository.FinalizarItensPendentesAsync();

            _logger.LogInformation(
                "Finalização concluída: {Quantidade} item(ns) de recolhimento finalizado(s)",
                quantidadeFinalizados);

            _logger.LogDebug("537-00-FINALIZA-ITEM-REC-PEND concluído");

            return new FinalizarItemRecolhimentoOutputDto
            {
                ExistiamItensPendentes = true,
                QuantidadeItensFinalizados = quantidadeFinalizados,
                Mensagem = $"{quantidadeFinalizados} item(ns) de recolhimento finalizado(s) com sucesso"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao finalizar itens de recolhimento pendentes");
            throw;
        }
    }
}
