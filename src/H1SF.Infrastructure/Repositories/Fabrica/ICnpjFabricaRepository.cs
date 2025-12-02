using H1SF.Domain.Entities.Fabrica;

namespace H1SF.Infrastructure.Repositories.Fabrica;

/// <summary>
/// 572-00-SQL-RECUPERA-CNPJ - Interface do repositório para recuperação de CNPJ
/// Linhas COBOL: 4469-4555
/// </summary>
public interface ICnpjFabricaRepository
{
    /// <summary>
    /// 572-10-SQL-CNPJ-VENDA
    /// Recupera CNPJ da fábrica para operação de venda normal
    /// </summary>
    Task<CnpjFabrica?> ObterCnpjVendaAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc);

    /// <summary>
    /// 572-20-SQL-CNPJ-TRIANG
    /// Recupera CNPJ da fábrica para operação triangular
    /// </summary>
    Task<CnpjFabrica?> ObterCnpjTriangulacaoAsync(
        int cdMercDst, 
        DateTime dtcSelFtrm, 
        string lgonFunc, 
        string icSim);
}
