using H1SF.Application.DTOs.DataHora;

namespace H1SF.Application.Services.DataHora;

/// <summary>
/// Interface para serviço de recuperação de data/hora (510-00-RECUPERA-DATA-HORA)
/// </summary>
public interface IRecuperadorDataHora
{
    /// <summary>
    /// Recupera a data e hora atual do sistema no formato COBOL
    /// COBOL: SELECT TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') FROM DUAL
    /// </summary>
    /// <returns>DTO com data/hora formatada</returns>
    Task<DataHoraSistemaDto> RecuperarDataHoraAsync();
}
