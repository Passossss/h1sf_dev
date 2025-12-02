using H1SF.Domain.Entities.Faturamento;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services;

/// <summary>
/// Serviço principal de processamento de faturamento
/// </summary>
public class ProcessadorFaturamentoService : IProcessadorFaturamentoService
{
    private readonly IProcessadorFaturamento _processadorParametros;
    private readonly IAtualizadorMonitor _atualizadorMonitor;
    private readonly ILogger<ProcessadorFaturamentoService> _logger;
    
    public ProcessadorFaturamentoService(
        IProcessadorFaturamento processadorParametros,
        IAtualizadorMonitor atualizadorMonitor,
        ILogger<ProcessadorFaturamentoService> logger)
    {
        _processadorParametros = processadorParametros;
        _atualizadorMonitor = atualizadorMonitor;
        _logger = logger;
    }
    
    /// <summary>
    /// Processa faturamento
    /// PERFORM 570-00-RETRIEVE-PARAMETRO
    /// PERFORM 560-00-ATUALIZA-MONITOR
    /// </summary>
    public async Task<FaturamentoParametros> ProcessarFaturamentoAsync(string parametrosEntrada)
    {
        _logger.LogInformation("Processamento iniciado");
        
        // PERFORM 570-00-RETRIEVE-PARAMETRO
        var parametros = _processadorParametros.RetrieveParametro(parametrosEntrada);
        
        // PERFORM 560-00-ATUALIZA-MONITOR
        await _atualizadorMonitor.AtualizarMonitorAsync(parametros);
        
        // IF WS31-CHV-CANCELADO NOT EQUAL SPACES
        if (_atualizadorMonitor.FoiCancelado)
        {
            _logger.LogWarning("Processamento cancelado");
            throw new InvalidOperationException("Processamento cancelado - verificar monitor");
        }
        
        _logger.LogInformation("Processamento concluído");
        return parametros;
    }
}
