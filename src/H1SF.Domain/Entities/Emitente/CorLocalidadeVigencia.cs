namespace H1SF.Domain.Entities.Emitente;

/// <summary>
/// Tabela B8CC.COR_LOCALIDADE_VIGENCIA
/// </summary>
public class CorLocalidadeVigencia
{
    /// <summary>
    /// PFJ_CODIGO - Código da Pessoa Física/Jurídica
    /// </summary>
    public string PfjCodigo { get; set; } = string.Empty;

    /// <summary>
    /// LOC_CODIGO - Código da Localidade
    /// </summary>
    public string LocCodigo { get; set; } = string.Empty;

    /// <summary>
    /// MUN_CODIGO - Código do Município
    /// </summary>
    public string MunCodigo { get; set; } = string.Empty;

    /// <summary>
    /// LOGRADOURO - Endereço
    /// </summary>
    public string? Logradouro { get; set; }

    /// <summary>
    /// NUMERO - Número do endereço
    /// </summary>
    public string? Numero { get; set; }

    /// <summary>
    /// BAIRRO - Bairro
    /// </summary>
    public string? Bairro { get; set; }

    /// <summary>
    /// CEP - CEP
    /// </summary>
    public string? Cep { get; set; }

    /// <summary>
    /// MUNICIPIO - Nome do Município
    /// </summary>
    public string? Municipio { get; set; }

    /// <summary>
    /// INSCR_ESTADUAL - Inscrição Estadual
    /// </summary>
    public string? InscrEstadual { get; set; }

    /// <summary>
    /// COMPLEMENTO - Complemento do endereço
    /// </summary>
    public string? Complemento { get; set; }

    /// <summary>
    /// DT_INICIO - Data de início da vigência
    /// </summary>
    public DateTime DtInicio { get; set; }

    /// <summary>
    /// DT_FIM - Data de fim da vigência
    /// </summary>
    public DateTime? DtFim { get; set; }
}
