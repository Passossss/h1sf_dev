namespace H1SF.Domain.Entities.Emitente;

/// <summary>
/// DTO para retorno da consulta 505-00-RECUPERA-EMITENTE
/// </summary>
public class DadosEmitente
{
    /// <summary>
    /// Razão Social em maiúsculas
    /// </summary>
    public string RazaoSocial { get; set; } = string.Empty;

    /// <summary>
    /// Razão Social com primeira letra maiúscula (INITCAP)
    /// </summary>
    public string RazaoSocialPm { get; set; } = string.Empty;

    /// <summary>
    /// Endereço completo para documentos
    /// </summary>
    public string EnderecoEd { get; set; } = string.Empty;

    /// <summary>
    /// Endereço completo para protocolo
    /// </summary>
    public string EnderecoEp { get; set; } = string.Empty;

    /// <summary>
    /// Endereço completo para item
    /// </summary>
    public string EnderecoEi { get; set; } = string.Empty;

    /// <summary>
    /// Bairro para protocolo
    /// </summary>
    public string BairroEp { get; set; } = string.Empty;

    /// <summary>
    /// Bairro e CEP para documento
    /// </summary>
    public string BairroCepEd { get; set; } = string.Empty;

    /// <summary>
    /// Bairro, Município, Nome e UF para protocolo
    /// </summary>
    public string BairroMunNomeUfEp { get; set; } = string.Empty;

    /// <summary>
    /// Bairro, Município, Nome e UF para item
    /// </summary>
    public string BairroMunNomeUfEi { get; set; } = string.Empty;

    /// <summary>
    /// CEP do emitente
    /// </summary>
    public string CepE { get; set; } = string.Empty;

    /// <summary>
    /// Município, Nome e UF
    /// </summary>
    public string MunNomeUfE { get; set; } = string.Empty;

    /// <summary>
    /// UF do emitente
    /// </summary>
    public string UfE { get; set; } = string.Empty;

    /// <summary>
    /// CPF/CGC formatado com texto
    /// </summary>
    public string CpfCgcE { get; set; } = string.Empty;

    /// <summary>
    /// Inscrição Estadual formatada com texto
    /// </summary>
    public string InscrEstadualE { get; set; } = string.Empty;

    /// <summary>
    /// CPF/CGC sem formatação
    /// </summary>
    public string CpfCgc { get; set; } = string.Empty;

    /// <summary>
    /// Inscrição Estadual sem formatação
    /// </summary>
    public string InscrEstadual { get; set; } = string.Empty;

    /// <summary>
    /// CPF/CGC e Inscrição Estadual juntos para item
    /// </summary>
    public string CpfCgcInscrEstEi { get; set; } = string.Empty;

    /// <summary>
    /// Complemento do endereço
    /// </summary>
    public string Complemento { get; set; } = string.Empty;
}
