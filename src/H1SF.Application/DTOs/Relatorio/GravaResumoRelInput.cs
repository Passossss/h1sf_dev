using H1SF.Application.DTOs.Relatorio;

namespace H1SF.Application.DTOs.Relatorio;

/// <summary>
/// Input para 550-00-GRAVA-RESUMO-REL
/// </summary>
public class GravaResumoRelInput
{
    // ST0001-CD-REGR-FTRM
    public string CodigoRegraFaturamento { get; set; } = string.Empty;

    // CB0004-CD-T-PRD
    public string CodigoTipoProduto { get; set; } = string.Empty;

    // CB0001-RRE-ID-PRCP-PTD-LIT
    public string IdPrincipalProtocoloLiteral { get; set; } = string.Empty;

    // CB0001-RRE-ID-REL
    public string IdRelatorio { get; set; } = string.Empty;

    // WS31-CHV-COMANDO-PJL
    public string ChaveComandoPjl { get; set; } = string.Empty;

    // CB0001-RRE-CD-STM
    public string CodigoSistema { get; set; } = string.Empty;

    // CB0001-RRE-DTC-GRC
    public string DataHoraGeracao { get; set; } = string.Empty;

    // CB0001-RRE-ID-PRCP
    public string IdPrincipal { get; set; } = string.Empty;

    // CB0001-RRE-CD-SQN-DCT
    public string CodigoSequenciaDocumento { get; set; } = string.Empty;

    // CB0001-RRE-ID-AUX-IMPS-1 a 5
    public string IdAuxiliarImpressao1 { get; set; } = string.Empty;
    public string IdAuxiliarImpressao2 { get; set; } = string.Empty;
    public string IdAuxiliarImpressao3 { get; set; } = string.Empty;
    public string IdAuxiliarImpressao4 { get; set; } = string.Empty;
    public string IdAuxiliarImpressao5 { get; set; } = string.Empty;

    // CB0001-RRE-ID-IMPR
    public string IdImpressora { get; set; } = string.Empty;
}

/// <summary>
/// Output para 550-00-GRAVA-RESUMO-REL
/// </summary>
public class GravaResumoRelOutput
{
    public bool Sucesso { get; set; }
    public bool ExecutouBypass { get; set; }
    public string ChaveComandoPjlFinal { get; set; } = string.Empty;
    public int LinhasAfetadas { get; set; }
}
