using H1SF.Domain.Entities.LogCaps;

namespace H1SF.Application.Services.LogCaps;

/// <summary>
/// Interface para 875-00-MONTA-LOG-CAPS
/// Gera log de sincronização CAPS (interface MQ)
/// </summary>
public interface IMontadorLogCaps
{
    /// <summary>
    /// 875-00-MONTA-LOG-CAPS SECTION
    /// Monta log de sincronização CAPS e grava em fila MQ
    /// </summary>
    Task MontarLogCapsAsync(MontarLogCapsInputDto input);
}

/// <summary>
/// Input para montagem do log CAPS
/// </summary>
public class MontarLogCapsInputDto
{
    public string CodigoMercadoDestino { get; set; } = string.Empty;
    public string DataSelecaoFaturamento { get; set; } = string.Empty;
    public string LoginFuncionario { get; set; } = string.Empty;
    public string FaseFaturamento { get; set; } = string.Empty;
    public string NumeroContabil { get; set; } = string.Empty; // CC0001-NUMERO-CTB-5
    public string AreaContaFila { get; set; } = string.Empty; // MQ01-ARE-CNT-FILA
}
