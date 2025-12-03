using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Infrastructure.Repositories.Recolhimento;

/// <summary>
/// Repository para 537-00-FINALIZA-ITEM-REC-PEND
/// Finaliza itens de recolhimento pendentes
/// </summary>
public interface IFinalizadorItemRecolhimentoRepository
{
    /// <summary>
    /// 537-00-FINALIZA-ITEM-REC-PEND
    /// Verifica se existem itens de recolhimento pendentes de finalização
    /// e atualiza DTC_FNL_ITEM com a data de finalização do faturamento
    /// </summary>
    Task<int> FinalizarItensPendentesAsync();
    
    /// <summary>
    /// Verifica se existem itens pendentes
    /// </summary>
    Task<bool> ExistemItensPendentesAsync();
}
