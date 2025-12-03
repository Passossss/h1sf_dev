using H1SF.Domain.Entities.Faturamento;
using H1SF.Domain.Entities.LogCaps;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Infrastructure.Repositories.LogCaps;

/// <summary>
/// Implementação do repository para 875-00-MONTA-LOG-CAPS
/// </summary>
public class LogCapsRepository : ILogCapsRepository
{
    private readonly ApplicationDbContext _context;

    public LogCapsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 875-10-RECUPERA-TOTAL
    /// EXEC SQL SELECT A.SUP_CD_FORN_SPR INTO :SF0009-SUP-CD-FORN-SPR
    /// FROM H1SF.SUP_FORNECEDOR A, H1ST.ITEM_RECOLHIMENTO B, H1SF.ITD_ITMFATURADO C
    /// </summary>
    public async Task<(string CodigoFornecedor, decimal TotalSelecao)> RecuperarTotalSelecaoAsync(
        string codigoMercadoDestino,
        string dataSelecaoFaturamento,
        string loginFuncionario,
        string faseFaturamento)
    {
        // Busca código do fornecedor
        var codigoFornecedor = await (
            from fornecedor in _context.Set<Fornecedor>()
            join itemRecolhimento in _context.ItensRecolhimento
                on fornecedor.CodigoFonteAtendimento equals itemRecolhimento.CodigoFonteAtendimento
            join itemFaturado in _context.ItensFaturados
                on itemRecolhimento.IdEtiqRec equals itemFaturado.IdEtiquetaRecolhimento
            where itemFaturado.CodigoMercadoDestino == codigoMercadoDestino
                && itemFaturado.DataSelecaoFaturamento == dataSelecaoFaturamento
                && itemFaturado.LoginFuncionario == loginFuncionario
                && ((faseFaturamento == "1" && itemFaturado.IdNotaFiscalReferencia == null)
                    || (faseFaturamento == "2" && itemFaturado.IdNotaFiscalReferencia != null))
            select fornecedor.CodigoFornecedorSuprimentos
        ).FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(codigoFornecedor))
        {
            throw new InvalidOperationException("*** ERRO - SUPPLIER CODE NAO ENCONTRADO   ===>");
        }

        // Calcula total da seleção
        // SELECT NVL(SUM(ROUND(ITD_Q_PECA_FTRD * ((ITD_PREC_FTRD_UNT_EMIF / 100) / (ITD_RATE_US / 10000)), 2)),0)
        var totalSelecao = await _context.ItensFaturados
            .Where(x => x.CodigoMercadoDestino == codigoMercadoDestino
                && x.DataSelecaoFaturamento == dataSelecaoFaturamento
                && x.LoginFuncionario == loginFuncionario)
            .Select(x => Math.Round(
                x.QuantidadePecaFaturada *
                ((x.PrecoFaturadoUnitarioEmissaoFiscal / 100m) /
                 (x.TaxaCambioUS / 10000m)), 2))
            .SumAsync();

        return (codigoFornecedor, totalSelecao);
    }

    /// <summary>
    /// 875-20-RECUPERA-DETALHE
    /// EXEC SQL DECLARE CSR_SEL_870 CURSOR FOR SELECT ...
    /// </summary>
    public async Task<List<LogCapsDetalheDto>> RecuperarDetalhesItensAsync(
        string codigoMercadoDestino,
        string dataSelecaoFaturamento,
        string loginFuncionario)
    {
        var query = from itemFaturado in _context.ItensFaturados
                    join peca in _context.Set<Peca>()
                        on itemFaturado.IdPeca equals peca.IdPeca
                    where itemFaturado.CodigoMercadoDestino == codigoMercadoDestino
                        && itemFaturado.DataSelecaoFaturamento == dataSelecaoFaturamento
                        && itemFaturado.LoginFuncionario == loginFuncionario
                    select new LogCapsDetalheDto
                    {
                        // TO_CHAR(A.ITD_DTC_FTR_EXP, 'YYYYMMDDHH24MISS')
                        DataFaturaExportacao = itemFaturado.DataFaturaExportacao != null
                            ? itemFaturado.DataFaturaExportacao.Value.ToString("yyyyMMddHHmmss")
                            : string.Empty,
                        NumeroFaturaExportacao = itemFaturado.NumeroFaturaExportacao ?? string.Empty,
                        NumeroPedido = itemFaturado.IdPedido ?? string.Empty,
                        ReferenciaCliente = itemFaturado.IdClienteReferencia ?? string.Empty,
                        CodigoPeca = itemFaturado.IdPeca,
                        QuantidadeFaturada = itemFaturado.QuantidadePecaFaturada,
                        // NVL(ROUND((A.ITD_PREC_FTRD_UNT_EMIF / 100) / (A.ITD_RATE_US / 10000), 5),0)
                        PrecoUnitario = Math.Round(
                            (itemFaturado.PrecoFaturadoUnitarioEmissaoFiscal / 100m) /
                            (itemFaturado.TaxaCambioUS / 10000m), 5),
                        // NVL(ROUND(A.ITD_Q_PECA_FTRD * ((A.ITD_PREC_FTRD_UNT_EMIF / 100) / (A.ITD_RATE_US / 10000)), 2),0)
                        PrecoTotal = Math.Round(
                            itemFaturado.QuantidadePecaFaturada *
                            ((itemFaturado.PrecoFaturadoUnitarioEmissaoFiscal / 100m) /
                             (itemFaturado.TaxaCambioUS / 10000m)), 2),
                        NomePecaIngles = peca.NomePecaIngles,
                        DataSelecaoFaturamento = itemFaturado.DataSelecaoFaturamento
                    };

        return await query.ToListAsync();
    }
}
