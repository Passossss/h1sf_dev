using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Application.Services;

public interface IProcessadorFaturamentoService
{
    Task<FaturamentoParametros> ProcessarFaturamentoAsync(string parametrosEntrada);
}
