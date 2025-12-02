using H1SF.Domain.Entities.Fabrica;

namespace H1SF.Application.Services.Fabrica;

/// <summary>
/// 572-00-SQL-RECUPERA-CNPJ - Interface do serviço de recuperação de CNPJ
/// Linhas COBOL: 4469-4555
/// </summary>
public interface IRecuperadorCnpjFabrica
{
    /// <summary>
    /// Recupera CNPJ da fábrica baseado no tipo de operação (venda ou triangulação)
    /// </summary>
    Task<CnpjFabrica?> RecuperarCnpjAsync(
        string cdMercDst,
        string faseFtrm,
        int cdMercDstInt,
        DateTime dtcSelFtrm,
        string lgonFunc,
        string icSim);
}
