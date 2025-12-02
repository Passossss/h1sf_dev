using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Application.Services;

public interface IAtualizadorMonitor
{
    bool FoiCancelado { get; }
    Task AtualizarMonitorAsync(FaturamentoParametros parametros);
}
