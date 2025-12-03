using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Infrastructure.Repositories.Recolhimento;

/// <summary>
/// Implementação do repository para 537-00-FINALIZA-ITEM-REC-PEND
/// </summary>
public class FinalizadorItemRecolhimentoRepository : IFinalizadorItemRecolhimentoRepository
{
    private readonly ApplicationDbContext _context;

    public FinalizadorItemRecolhimentoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Verifica se existem itens de recolhimento pendentes
    /// SELECT 1 FROM H1ST.ITEM_RECOLHIMENTO A
    /// WHERE A.DTC_FNL_REC IS NOT NULL AND A.DTC_FNL_ITEM IS NULL
    /// AND EXISTS (SELECT 1 FROM H1SE.V_ITD_ITMFATURADO B, H1SE.V_MNT_MONITOR_FTRM C ...)
    /// </summary>
    public async Task<bool> ExistemItensPendentesAsync()
    {
        var existem = await (
            from itemRec in _context.ItensRecolhimento
            where itemRec.DtcFnlRec != null
                && itemRec.DtcFnlItem == null
                && _context.ItensFaturados.Any(itemFat =>
                    itemFat.IdEtiquetaRecolhimento == itemRec.IdEtiqRec
                    && _context.MonitorFaturamento.Any(monitor =>
                        monitor.CodigoMercadoriaDestino.ToString() == itemFat.CodigoMercadoDestino
                        && monitor.TimestampSelecao == itemFat.DataSelecaoFaturamento
                        && monitor.LoginFuncionario == itemFat.LoginFuncionario
                        && monitor.CodigoTipoRecolhimento == itemFat.ItdCdTRec
                        && monitor.DataFinalizacaoFaturamento != null))
            select itemRec
        ).AnyAsync();

        return existem;
    }

    /// <summary>
    /// 537-00-FINALIZA-ITEM-REC-PEND
    /// UPDATE H1ST.ITEM_RECOLHIMENTO A SET A.DTC_FNL_ITEM = (SELECT C.MNT_DTC_FNL_FTRM ...)
    /// WHERE A.DTC_FNL_REC IS NOT NULL AND A.DTC_FNL_ITEM IS NULL
    /// AND EXISTS (SELECT 1 FROM ... WHERE C.MNT_DTC_FNL_FTRM IS NOT NULL)
    /// </summary>
    public async Task<int> FinalizarItensPendentesAsync()
    {
        // Buscar itens pendentes com suas respectivas datas de finalização
        var itensPendentes = await (
            from itemRec in _context.ItensRecolhimento
            where itemRec.DtcFnlRec != null
                && itemRec.DtcFnlItem == null
            let dataFinalizacao = (
                from itemFat in _context.ItensFaturados
                join monitor in _context.MonitorFaturamento
                    on new { 
                        CodigoMercado = itemFat.CodigoMercadoDestino,
                        DataSelecao = itemFat.DataSelecaoFaturamento,
                        Login = itemFat.LoginFuncionario,
                        TipoRec = itemFat.ItdCdTRec
                    }
                    equals new { 
                        CodigoMercado = monitor.CodigoMercadoriaDestino.ToString(),
                        DataSelecao = monitor.TimestampSelecao,
                        Login = monitor.LoginFuncionario,
                        TipoRec = monitor.CodigoTipoRecolhimento ?? 0
                    }
                where itemFat.IdEtiquetaRecolhimento == itemRec.IdEtiqRec
                    && monitor.DataFinalizacaoFaturamento != null
                select monitor.DataFinalizacaoFaturamento
            ).FirstOrDefault()
            where dataFinalizacao != null
            select new { ItemRecolhimento = itemRec, DataFinalizacao = dataFinalizacao }
        ).ToListAsync();

        // Atualizar cada item
        foreach (var item in itensPendentes)
        {
            item.ItemRecolhimento.DtcFnlItem = item.DataFinalizacao;
        }

        // Salvar mudanças
        if (itensPendentes.Any())
        {
            await _context.SaveChangesAsync();
        }

        return itensPendentes.Count;
    }
}
