using H1SF.Domain.Entities.LogCaps;

namespace H1SF.Infrastructure.Repositories.LogCaps;

/// <summary>
/// Repository para 875-00-MONTA-LOG-CAPS
/// Interface para geração de log CAPS
/// </summary>
public interface ILogCapsRepository
{
    /// <summary>
    /// 875-10-RECUPERA-TOTAL
    /// Busca código do fornecedor e calcula total da seleção
    /// </summary>
    Task<(string CodigoFornecedor, decimal TotalSelecao)> RecuperarTotalSelecaoAsync(
        string codigoMercadoDestino,
        string dataSelecaoFaturamento,
        string loginFuncionario,
        string faseFaturamento);
    
    /// <summary>
    /// 875-20-RECUPERA-DETALHE
    /// Busca detalhes dos itens faturados para montagem do log
    /// </summary>
    Task<List<LogCapsDetalheDto>> RecuperarDetalhesItensAsync(
        string codigoMercadoDestino,
        string dataSelecaoFaturamento,
        string loginFuncionario);
}

/// <summary>
/// Dados detalhados de cada item para o log CAPS
/// </summary>
public class LogCapsDetalheDto
{
    public string DataFaturaExportacao { get; set; } = string.Empty; // ITD_DTC_FTR_EXP formatado
    public string NumeroFaturaExportacao { get; set; } = string.Empty; // ITD_FTR_EXP
    public string NumeroPedido { get; set; } = string.Empty; // ITD_ID_PDD
    public string ReferenciaCliente { get; set; } = string.Empty; // ITD_ID_CLI_REF
    public string CodigoPeca { get; set; } = string.Empty; // ITD_ID_PECA
    public int QuantidadeFaturada { get; set; } // ITD_Q_PECA_FTRD
    public decimal PrecoUnitario { get; set; } // Calculado
    public decimal PrecoTotal { get; set; } // Calculado
    public string NomePecaIngles { get; set; } = string.Empty; // NM_PECA_ING
    public string DataSelecaoFaturamento { get; set; } = string.Empty; // ITD_DTC_SEL_FTRM
}
