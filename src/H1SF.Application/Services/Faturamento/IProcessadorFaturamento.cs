using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Application.Services;

public interface IProcessadorFaturamento
{
    FaturamentoParametros RetrieveParametro(string parametrosEntrada);
}
