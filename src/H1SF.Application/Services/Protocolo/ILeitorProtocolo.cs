using H1SF.Domain.Entities.Protocolo;

namespace H1SF.Application.Services.Protocolo;

/// <summary>
/// 500-00-LE-PROTOCOLO - Interface do serviço de leitura de protocolos
/// Linhas COBOL: 3278-3285
/// </summary>
public interface ILeitorProtocolo
{
    /// <summary>
    /// Lê e processa protocolos de despacho
    /// </summary>
    Task<List<DadosProtocolo>> ProcessarProtocolosAsync(
        int cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc);
}
