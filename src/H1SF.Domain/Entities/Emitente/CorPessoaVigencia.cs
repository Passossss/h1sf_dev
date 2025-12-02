namespace H1SF.Domain.Entities.Emitente;

/// <summary>
/// Tabela B8CC.COR_PESSOA_VIGENCIA
/// </summary>
public class CorPessoaVigencia
{
    /// <summary>
    /// PFJ_CODIGO - Código da Pessoa Física/Jurídica
    /// </summary>
    public string PfjCodigo { get; set; } = string.Empty;

    /// <summary>
    /// CPF_CGC - CPF/CNPJ
    /// </summary>
    public string? CpfCgc { get; set; }

    /// <summary>
    /// DT_INICIO - Data de início da vigência
    /// </summary>
    public DateTime DtInicio { get; set; }

    /// <summary>
    /// DT_FIM - Data de fim da vigência
    /// </summary>
    public DateTime? DtFim { get; set; }
}
