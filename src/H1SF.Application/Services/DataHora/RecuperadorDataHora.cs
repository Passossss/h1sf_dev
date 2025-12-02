using H1SF.Application.DTOs.DataHora;
using H1SF.Infrastructure.Repositories.DataHora;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.DataHora;

/// <summary>
/// 510-00-RECUPERA-DATA-HORA - Recupera data e hora do sistema
/// Linhas COBOL: 3386-3405
/// Autor: A.C.ANDREATTA
/// </summary>
public class RecuperadorDataHora : IRecuperadorDataHora
{
    private readonly IDataHoraRepository _repository;
    private readonly ILogger<RecuperadorDataHora> _logger;
    
    public RecuperadorDataHora(
        IDataHoraRepository repository,
        ILogger<RecuperadorDataHora> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    /// <summary>
    /// 510-00-RECUPERA-DATA-HORA SECTION
    /// EXEC SQL SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') INTO :CB0001-RRE-DTC-GRC FROM DUAL
    /// </summary>
    public async Task<DataHoraSistemaDto> RecuperarDataHoraAsync()
    {
        _logger.LogDebug("510-00-RECUPERA-DATA-HORA iniciado");
        
        try
        {
            // EXEC SQL SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') INTO :CB0001-RRE-DTC-GRC FROM DUAL
            var dataHoraFormatada = await _repository.ObterDataHoraSistemaAsync();
            
            // IF SQLCODE EQUAL WS31-CHV-NTFD-SQL (validação de erro)
            if (string.IsNullOrEmpty(dataHoraFormatada))
            {
                var msgErro = "*** ERRO - TABELA DUAL SEM REGISTRO  ===>";
                _logger.LogError("H1SF0033: {Mensagem}", msgErro);
                throw new InvalidOperationException(msgErro);
            }
            
            // Parsear a string para DateTime
            // Formato: YYYYMMDDHH24MISS = 20231215143025
            if (!DateTime.TryParseExact(
                dataHoraFormatada,
                "yyyyMMddHHmmss",
                null,
                System.Globalization.DateTimeStyles.None,
                out var dataHora))
            {
                throw new FormatException($"Formato de data/hora inválido: {dataHoraFormatada}");
            }
            
            _logger.LogDebug("Data/Hora recuperada: {DataHora}", dataHoraFormatada);
            
            return new DataHoraSistemaDto
            {
                DataHoraFormatada = dataHoraFormatada,
                DataHora = dataHora
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar data/hora do sistema");
            throw;
        }
    }
}
