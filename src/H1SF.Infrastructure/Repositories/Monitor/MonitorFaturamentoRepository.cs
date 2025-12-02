using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace H1SF.Infrastructure.Repositories;

public class MonitorFaturamentoRepository : IMonitorFaturamentoRepository
{
    private readonly ApplicationDbContext _context;

    public MonitorFaturamentoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MonitorFaturamento?> ObterMonitorAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
    {
        // Corresponde ao SELECT do COBOL para verificar o monitor
        return await _context.MonitorFaturamento
            .Where(x => x.CodigoMercadoriaDestino == cdMercDst &&
                       x.TimestampSelecao == dtcSelFtrm &&
                       x.LoginFuncionario == lgonFunc)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> VerificarCancelamentoAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
    {
        // Corresponde ao 560-10-VERIFICA-CANCELAMENTO
        var monitor = await _context.MonitorFaturamento
            .Where(x => x.CodigoMercadoriaDestino == cdMercDst &&
                       x.TimestampSelecao == dtcSelFtrm &&
                       x.LoginFuncionario == lgonFunc)
            .Select(x => new { x.MotivoInterrupcao, x.IndicadorCancelamento })
            .FirstOrDefaultAsync();

        if (monitor == null)
            return false;

        // Verifica se há interrupção (MNT_ID_MTV_ITRP NOT = SPACES) ou cancelamento (MNT_IC_CAN = 'S')
        return (monitor.MotivoInterrupcao.HasValue && monitor.MotivoInterrupcao != ' ') ||
               (monitor.IndicadorCancelamento.HasValue && monitor.IndicadorCancelamento == 'S');
    }

    public async Task AtualizarFaseAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc, DateTime dtcFase, int fase)
    {
        // Corresponde ao 560-20-ATUALIZA-FASE
        var monitor = await _context.MonitorFaturamento
            .Where(x => x.CodigoMercadoriaDestino == cdMercDst &&
                       x.TimestampSelecao == dtcSelFtrm &&
                       x.LoginFuncionario == lgonFunc)
            .FirstOrDefaultAsync();

        if (monitor != null)
        {
            if (fase == 1)
            {
                monitor.DataFaseMontagem = dtcFase;
            }
            else if (fase == 2)
            {
                monitor.DataFaseComplementar = dtcFase;
            }

            await _context.SaveChangesAsync();
        }
    }
}
