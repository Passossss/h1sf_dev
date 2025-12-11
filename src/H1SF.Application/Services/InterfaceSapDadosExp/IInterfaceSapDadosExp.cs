using H1SF.Application.DTOs.InterfaceSapDadosExp;

namespace H1SF.Application.Services.InterfaceSapDadosExp;

/// <summary>
/// Interface para 830-00-INTERFACE-SAP-DADOS-EXP
/// </summary>
public interface IInterfaceSapDadosExp
{
    /// <summary>
    /// Executa interface SAP para dados de exportação
    /// </summary>
    Task<InterfaceSapDadosExpOutput> ExecutarAsync(InterfaceSapDadosExpInput input);
}
