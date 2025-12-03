namespace H1SF.Application.Services.Monitor;

/// <summary>
/// Interface para 565-00-ATUALIZA-LBRC-IMPS
/// Atualiza fase LBRC_IMPS no monitor de faturamento
/// </summary>
public interface IAtualizadorFaseLbrcImps
{
    /// <summary>
    /// 565-00-ATUALIZA-LBRC-IMPS SECTION
    /// UPDATE H1SF.MNT_MONITOR_FTRM SET MNT_DTC_FASE_LBRC_IMPS = SYSDATE
    /// </summary>
    Task AtualizarFaseLbrcImpsAsync(AtualizarFaseLbrcImpsInputDto input);
}

/// <summary>
/// Input para atualizar fase LBRC_IMPS
/// </summary>
public class AtualizarFaseLbrcImpsInputDto
{
    /// <summary>
    /// WQ02-CD-MERC-DST - Código do mercado destino
    /// </summary>
    public string CodigoMercadoDestino { get; set; } = string.Empty;
    
    /// <summary>
    /// WQ02-DTC-SEL-FTRM - Data/timestamp da seleção de faturamento
    /// </summary>
    public string DataSelecaoFaturamento { get; set; } = string.Empty;
    
    /// <summary>
    /// WQ02-LGON-FUNC - Login do funcionário
    /// </summary>
    public string LoginFuncionario { get; set; } = string.Empty;
}
