using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Infrastructure.Repositories;

public interface IDefinirImpressoraRepository
{
    Task<int?> ObterIdTipoRecolhimentoAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc);
    Task<SelecaoFaturamento?> ObterSelecaoFaturamentoAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc);
    Task<string?> ObterNomeImpressoraAsync(int idImpressora);
    Task<bool> ValidarImpressoraExisteAsync(int idImpressora);
}
