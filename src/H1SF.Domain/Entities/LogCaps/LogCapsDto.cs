namespace H1SF.Domain.Entities.LogCaps;

/// <summary>
/// DTO para o layout SF80042-ASN-LAYOUT
/// Representa mensagem para interface CAPS (MQ)
/// </summary>
public class LogCapsDto
{
    public int IdSequencia { get; set; }
    public string TipoRegistro { get; set; } = string.Empty; // 'DN' ou 'ST'
    public string CodigoAcesso { get; set; } = string.Empty;
    
    // Dados do cabeçalho
    public string CodigoFabricaRecebimento { get; set; } = string.Empty; // 'ZQ'
    public string CodigoFornecedor { get; set; } = string.Empty;
    public decimal TotalFatura { get; set; }
    public string CodigoMoeda { get; set; } = string.Empty; // 'USD'
    
    // Dados de data
    public string DataFaturaCenturiaCC3 { get; set; } = string.Empty;
    public string DataFaturaAnoYY3 { get; set; } = string.Empty;
    public string DataFaturaMesDiaMmdd3 { get; set; } = string.Empty;
    public string NumeroFatura { get; set; } = string.Empty;
    
    public string DataProcessamentoCenturiaCC4 { get; set; } = string.Empty;
    public string DataProcessamentoAnoYY4 { get; set; } = string.Empty;
    public string DataProcessamentoMesDiaMmdd4 { get; set; } = string.Empty;
    
    // Dados da linha de item
    public string ReferenciaPedido { get; set; } = string.Empty;
    public string ReferenciaCliente { get; set; } = string.Empty;
    public string TipoLinhaFatura { get; set; } = string.Empty; // '001'
    public string CodigoCatalogo { get; set; } = string.Empty;
    public int QuantidadeFaturada { get; set; }
    public int UnidadeMedidaPreco { get; set; } // 3
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal { get; set; }
    public string DescricaoPeca { get; set; } = string.Empty;
    public string NumeroControleIntercambio { get; set; } = string.Empty;
    
    // Campos de controle MQ
    public string IdCorrelacaoMensagem { get; set; } = string.Empty;
    public int TamanhoMensagem { get; set; } = 461;
    public string ChavePadrao { get; set; } = string.Empty;
}
