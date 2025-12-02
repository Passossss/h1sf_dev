using H1SF.Domain.Entities.Protocolo;

namespace H1SF.Infrastructure.Repositories.Protocolo;

/// <summary>
/// 500-00-LE-PROTOCOLO - Interface do repositório para leitura de protocolos
/// Linhas COBOL: 3278-3285 (chama 700-00-TRATA-PROTOCOLO que chama 705-00-MONTA-DANFE)
/// </summary>
public interface IProtocoloRepository
{
    /// <summary>
    /// Lista protocolos de despacho não impressos (PTD_IC_DSP_IMPS = 'N')
    /// </summary>
    Task<List<DadosProtocolo>> ListarProtocolosNaoImpressosAsync(
        int cdMercDst, 
        DateTime dtcSelFtrm, 
        string lgonFunc);

    /// <summary>
    /// Atualiza protocolo como impresso (PTD_IC_DSP_IMPS = 'S')
    /// </summary>
    Task AtualizarProtocoloComoImpressoAsync(
        int cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc,
        int cdTRec,
        int cdTMtz,
        int idMtz,
        int idCli,
        string idPtcDsp);

    /// <summary>
    /// Verifica se existem itens faturados para o protocolo
    /// </summary>
    Task<bool> ExistemItensFaturadosAsync(
        int cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc,
        string idPtcDsp);
}
