using H1SF.Application.DTOs.Interface;

namespace H1SF.Application.Services;

/// <summary>
/// 880-00-INTERFACE-S-DE-ITEM
/// </summary>
public interface IInterfaceSDeItem880
{
    Task<InterfaceSDeItem880Output> ExecutarAsync(InterfaceSDeItem880Input input);
}
