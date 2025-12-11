using H1SF.Application.DTOs.InterfaceSdeFat;

namespace H1SF.Application.Services.InterfaceSdeFat;

/// <summary>
/// Interface para 885-00-INTERFACE-S-DE-FAT
/// </summary>
public interface IInterfaceSdeFat
{
    /// <summary>
    /// Executa interface S-DE para fatura
    /// </summary>
    Task<InterfaceSdeFatOutput> ExecutarAsync(InterfaceSdeFatInput input);
}
