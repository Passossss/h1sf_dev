using H1SF.Application.DTOs.Interface;

namespace H1SF.Application.Services;

/// <summary>
/// 855-00-INTERFACE-CME-ITEM
/// </summary>
public interface IInterfaceCmeItem855
{
    Task<InterfaceCmeItem855Output> ExecutarAsync(InterfaceCmeItem855Input input);
}
