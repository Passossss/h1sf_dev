namespace H1SF.Application.DTOs.EmiteLinkH1sf5053;

/// <summary>
/// Input para 583-00-EMITE-LINK-H1SF5053
/// </summary>
public class EmiteLinkH1sf5053Input
{
    // SF0002-ITD-FTR-EXP
    public string FaturaExportacao { get; set; } = string.Empty;
}

/// <summary>
/// Output para 583-00-EMITE-LINK-H1SF5053
/// </summary>
public class EmiteLinkH1sf5053Output
{
    public bool Sucesso { get; set; }
    public string CodigoAcesso { get; set; } = string.Empty;
    public string CodigoRetornoEci { get; set; } = string.Empty;
    public string CodigoRetornoAcesso { get; set; } = string.Empty;
    public string TipoDocumentoEletronico { get; set; } = string.Empty;
    public bool ProgramaExecutado { get; set; }
}
