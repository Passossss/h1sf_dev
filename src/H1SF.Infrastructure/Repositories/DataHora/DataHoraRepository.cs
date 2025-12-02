using Microsoft.Extensions.Logging;

namespace H1SF.Infrastructure.Repositories.DataHora;

/// <summary>
/// 510-00-RECUPERA-DATA-HORA - Recupera data e hora do sistema
/// Linhas COBOL: 2705-2721
/// </summary>
public class DataHoraRepository : IDataHoraRepository
{
    private readonly ILogger<DataHoraRepository> _logger;
    
    public DataHoraRepository(ILogger<DataHoraRepository> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// 510-00-RECUPERA-DATA-HORA SECTION
    /// EXEC SQL SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') INTO :CB0001-RRE-DTC-GRC FROM DUAL
    /// Retorna: YYYYMMDDHH24MISS (formato COBOL PIC X(014))
    /// </summary>
    public Task<string> ObterDataHoraSistemaAsync()
    {
        try
        {
            // COBOL: EXEC SQL SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')
            //                 INTO   :CB0001-RRE-DTC-GRC
            //                 FROM   DUAL
            
            // .NET equivalente: usar DateTime.Now e formatar
            var dataHoraAtual = DateTime.Now;
            var dataHoraFormatada = dataHoraAtual.ToString("yyyyMMddHHmmss");
            
            _logger.LogDebug("Data/Hora obtida: {DataHora}", dataHoraFormatada);
            
            return Task.FromResult(dataHoraFormatada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter data/hora do sistema");
            throw;
        }
    }
}
