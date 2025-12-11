namespace H1SF.Application.DTOs.InterfaceSapDadosExp;

/// <summary>
/// Input para 830-00-INTERFACE-SAP-DADOS-EXP
/// </summary>
public class InterfaceSapDadosExpInput
{
    // SF0002-ITD-FTR-EXP
    public string FaturaExportacao { get; set; } = string.Empty;
}

/// <summary>
/// Output para 830-00-INTERFACE-SAP-DADOS-EXP
/// </summary>
public class InterfaceSapDadosExpOutput
{
    public bool Sucesso { get; set; }
    public string CodigoAcesso { get; set; } = string.Empty;
    public string CodigoRetornoEci { get; set; } = string.Empty;
    public string CodigoRetornoAcesso { get; set; } = string.Empty;
    public string TipoDocumentoEletronico { get; set; } = string.Empty;
    public bool ProgramaExecutado { get; set; }
}
