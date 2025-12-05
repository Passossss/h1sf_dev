using H1SF.Domain.Entities.Faturamento;

namespace H1SF.Infrastructure.Repositories;

public interface IDefinirImpressoraRepository
{
    Task<int?> ObterIdTipoRecolhimentoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc);
    Task<SelecaoFaturamento?> ObterSelecaoFaturamentoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc);
    Task<string?> ObterNomeImpressoraAsync(int idImpressora);
    Task<bool> ValidarImpressoraExisteAsync(int idImpressora);
}
