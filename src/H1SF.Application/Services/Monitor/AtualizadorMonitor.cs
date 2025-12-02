using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services;

/// <summary>
/// 560-00-ATUALIZA-MONITOR - Atualiza monitor de faturamento
/// Linhas COBOL: 4282-4349
/// </summary>
public class AtualizadorMonitor : IAtualizadorMonitor
{
    private readonly IMonitorFaturamentoRepository _repository;
    private readonly ILogger<AtualizadorMonitor> _logger;
    
    // WS31-CHV-CANCELADO - Flag de cancelamento
    public bool FoiCancelado { get; private set; }
    
    public AtualizadorMonitor(
        IMonitorFaturamentoRepository repository,
        ILogger<AtualizadorMonitor> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    /// <summary>
    /// 560-00-ATUALIZA-MONITOR SECTION
    /// Verifica cancelamento e atualiza fase
    /// </summary>
    public async Task AtualizarMonitorAsync(FaturamentoParametros parametros)
    {
        _logger.LogDebug("560-00-ATUALIZA-MONITOR iniciado");
        
        // 560-10-VERIFICA-CANCELAMENTO
        if (await VerificaCancelamentoAsync(parametros))
        {
            FoiCancelado = true;
            _logger.LogWarning("Processamento cancelado");
            return; // 560-99-ATUALIZA-EXIT
        }
        
        // 560-20-ATUALIZA-FASE
        await AtualizaFaseAsync(parametros);
        
        // 560-30-EFETIVA-FASE
        EmiteSyncpoint();
        
        _logger.LogDebug("560-00-ATUALIZA-MONITOR concluído");
    }
    
    /// <summary>
    /// 560-10-VERIFICA-CANCELAMENTO
    /// Verifica se existe interrupção ou cancelamento
    /// </summary>
    private async Task<bool> VerificaCancelamentoAsync(FaturamentoParametros parametros)
    {
        _logger.LogDebug("560-10-VERIFICA-CANCELAMENTO");
        
        return await _repository.VerificarCancelamentoAsync(
            parametros.CodigoMercadoriaDestino,
            parametros.DataHoraSelecao,
            parametros.LoginFuncionario);
    }
    
    /// <summary>
    /// 560-20-ATUALIZA-FASE
    /// Atualiza a data/hora da fase no monitor
    /// </summary>
    private async Task AtualizaFaseAsync(FaturamentoParametros parametros)
    {
        _logger.LogDebug("560-20-ATUALIZA-FASE");
        
        var dtcFase = DateTime.Now;
        int fase = parametros.FaseFaturamento == '1' ? 1 : 2;
        
        await _repository.AtualizarFaseAsync(
            parametros.CodigoMercadoriaDestino,
            parametros.DataHoraSelecao,
            parametros.LoginFuncionario,
            dtcFase,
            fase);
    }
    
    /// <summary>
    /// 560-30-EFETIVA-FASE
    /// Emite SYNCPOINT (COMMIT)
    /// </summary>
    private void EmiteSyncpoint()
    {
        _logger.LogDebug("560-30-EFETIVA-FASE - SYNCPOINT emitido");
        // O commit é feito automaticamente pelo DbContext.SaveChangesAsync() no repository
    }
}
