using H1SF.Application.DTOs.Interface;

namespace H1SF.Application.Services;

/// <summary>
/// 865-00-INTERFACE-DEA-ITEM
/// </summary>
public interface IInterfaceDeaItem865
{
    Task<InterfaceDeaItem865Output> ExecutarAsync(InterfaceDeaItem865Input input);
}
