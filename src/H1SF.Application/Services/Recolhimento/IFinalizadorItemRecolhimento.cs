namespace H1SF.Application.Services.Recolhimento;

/// <summary>
/// Interface para 537-00-FINALIZA-ITEM-REC-PEND
/// Finaliza itens de recolhimento que estão pendentes
/// </summary>
public interface IFinalizadorItemRecolhimento
{
    /// <summary>
    /// 537-00-FINALIZA-ITEM-REC-PEND SECTION
    /// Atualiza DTC_FNL_ITEM dos itens de recolhimento pendentes
    /// com a data de finalização do faturamento (MNT_DTC_FNL_FTRM)
    /// </summary>
    Task<FinalizarItemRecolhimentoOutputDto> FinalizarItensPendentesAsync();
}

/// <summary>
/// Output da finalização de itens de recolhimento
/// </summary>
public class FinalizarItemRecolhimentoOutputDto
{
    /// <summary>
    /// Indica se foram encontrados itens para finalizar
    /// </summary>
    public bool ExistiamItensPendentes { get; set; }
    
    /// <summary>
    /// Quantidade de itens finalizados
    /// </summary>
    public int QuantidadeItensFinalizados { get; set; }
    
    /// <summary>
    /// Mensagem de resultado
    /// </summary>
    public string Mensagem { get; set; } = string.Empty;
}
