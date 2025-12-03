using H1SF.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Monitor;

/// <summary>
/// 565-00-ATUALIZA-LBRC-IMPS - Atualiza fase LBRC_IMPS
/// Linhas COBOL: aproximadamente 5500-5520
/// Autor: E. FRIOLI JR.
/// </summary>
public class AtualizadorFaseLbrcImps : IAtualizadorFaseLbrcImps
{
    private readonly IMonitorFaturamentoRepository _repository;
    private readonly ILogger<AtualizadorFaseLbrcImps> _logger;

    public AtualizadorFaseLbrcImps(
        IMonitorFaturamentoRepository repository,
        ILogger<AtualizadorFaseLbrcImps> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 565-00-ATUALIZA-LBRC-IMPS SECTION
    /// EXEC SQL UPDATE H1SF.MNT_MONITOR_FTRM
    /// SET MNT_DTC_FASE_LBRC_IMPS = SYSDATE
    /// WHERE MNT_CD_MERC_DST = :WQ02-CD-MERC-DST
    /// AND MNT_DTC_SEL_FTRM = :WQ02-DTC-SEL-FTRM
    /// AND MNT_LGON_FUNC = :WQ02-LGON-FUNC
    /// </summary>
    public async Task AtualizarFaseLbrcImpsAsync(AtualizarFaseLbrcImpsInputDto input)
    {
        _logger.LogDebug("565-00-ATUALIZA-LBRC-IMPS iniciado");

        try
        {
            _logger.LogInformation(
                "Atualizando fase LBRC_IMPS - Mercado: {Mercado}, Data: {Data}, Usuário: {Usuario}",
                input.CodigoMercadoDestino,
                input.DataSelecaoFaturamento,
                input.LoginFuncionario);

            await _repository.AtualizarFaseLbrcImpsAsync(
                input.CodigoMercadoDestino,
                input.DataSelecaoFaturamento,
                input.LoginFuncionario);

            _logger.LogInformation("Fase LBRC_IMPS atualizada com sucesso");
            _logger.LogDebug("565-00-ATUALIZA-LBRC-IMPS concluído");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar fase LBRC_IMPS");
            throw;
        }
    }
}
