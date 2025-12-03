using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Infrastructure.Repositories;

public interface IMonitorFaturamentoRepository
{
    Task<MonitorFaturamento?> ObterMonitorAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc);
    Task AtualizarFaseAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc, DateTime dtcFase, int fase);
    Task<bool> VerificarCancelamentoAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc);
    
    /// <summary>
    /// 565-00-ATUALIZA-LBRC-IMPS
    /// Atualiza MNT_DTC_FASE_LBRC_IMPS com SYSDATE
    /// </summary>
    Task AtualizarFaseLbrcImpsAsync(string codigoMercadoDestino, string dataSelecaoFaturamento, string loginFuncionario);
}
