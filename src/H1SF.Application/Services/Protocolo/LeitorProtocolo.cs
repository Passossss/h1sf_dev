using H1SF.Domain.Entities.Protocolo;
using H1SF.Infrastructure.Repositories.Protocolo;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Protocolo;

/// <summary>
/// 500-00-LE-PROTOCOLO - Lê protocolos de despacho
/// Linhas COBOL: 3278-3285 (chama 700-00-TRATA-PROTOCOLO)
/// </summary>
public class LeitorProtocolo : ILeitorProtocolo
{
    private readonly IProtocoloRepository _repository;
    private readonly ILogger<LeitorProtocolo> _logger;

    public LeitorProtocolo(
        IProtocoloRepository repository,
        ILogger<LeitorProtocolo> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 500-00-LE-PROTOCOLO SECTION
    /// PERFORM 700-00-TRATA-PROTOCOLO
    /// 
    /// 700-00-TRATA-PROTOCOLO SECTION
    /// PERFORM 705-00-MONTA-DANFE
    /// </summary>
    public async Task<List<DadosProtocolo>> ProcessarProtocolosAsync(
        string cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc)
    {
        try
        {
            _logger.LogDebug(
                "Processando protocolos: CdMercDst={CdMercDst}, DtcSelFtrm={DtcSelFtrm}, LgonFunc={LgonFunc}",
                cdMercDst, dtcSelFtrm, lgonFunc);

            // COBOL: 705-00-MONTA-DANFE - busca protocolos não impressos
            var protocolos = await _repository.ListarProtocolosNaoImpressosAsync(
                cdMercDst, dtcSelFtrm, lgonFunc);

            if (!protocolos.Any())
            {
                _logger.LogInformation("Nenhum protocolo não impresso encontrado");
                return new List<DadosProtocolo>();
            }

            var protocolosProcessados = new List<DadosProtocolo>();

            foreach (var protocolo in protocolos)
            {
                // COBOL: Verifica se existem itens faturados para o protocolo
                // EXEC SQL SELECT 1 FROM H1SF.ITD_ITMFATURADO WHERE ...
                var existemItens = await _repository.ExistemItensFaturadosAsync(
                    protocolo.PtdCdMercDst,
                    protocolo.PtdDtcSelFtrm,
                    protocolo.PtdLgonFunc,
                    protocolo.PtdIdPtcDsp);

                // COBOL: IF SQLCODE EQUAL WS31-CHV-NTFD-SQL
                //            EXEC SQL UPDATE H1SF.PTD_PROTODSP SET PTD_IC_DSP_IMPS = 'S'
                //            GO TO 705-05-LOOP-PROTOCOLO
                if (!existemItens)
                {
                    _logger.LogWarning(
                        "Protocolo sem itens faturados, marcando como impresso: IdPtcDsp={IdPtcDsp}",
                        protocolo.PtdIdPtcDsp);

                    await _repository.AtualizarProtocoloComoImpressoAsync(
                        protocolo.PtdCdMercDst,
                        protocolo.PtdDtcSelFtrm,
                        protocolo.PtdLgonFunc,
                        protocolo.PtdCdTRec ?? 0,
                        protocolo.PtdCdTMtz ?? 0,
                        protocolo.PtdIdMtz ?? 0,
                        protocolo.PtdIdCli ?? 0,
                        protocolo.PtdIdPtcDsp);

                    continue; // Pula para próximo protocolo
                }

                // COBOL: INSPECT SF0001-PTD-CD-TRSR CONVERTING SPACES TO ZEROS
                if (protocolo.PtdCdTrsr != null)
                {
                    protocolo.PtdCdTrsr = protocolo.PtdCdTrsr.Replace(" ", "0");
                }

                // COBOL: INSPECT SF0001-PTD-ID-PTC-DSP CONVERTING SPACES TO ZEROS
                protocolo.PtdIdPtcDsp = protocolo.PtdIdPtcDsp.Replace(" ", "0");

                protocolosProcessados.Add(protocolo);

                _logger.LogDebug(
                    "Protocolo processado: IdPtcDsp={IdPtcDsp}, Volumes={Volumes}, Valor={Valor}",
                    protocolo.PtdIdPtcDsp, protocolo.QuantidadeVolumes, protocolo.ValorTotalFormatado);
            }

            _logger.LogInformation(
                "Protocolos processados: {Count} de {Total}",
                protocolosProcessados.Count, protocolos.Count);

            return protocolosProcessados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar protocolos");
            throw;
        }
    }
}
