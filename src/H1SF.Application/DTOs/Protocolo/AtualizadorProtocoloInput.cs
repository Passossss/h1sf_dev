namespace H1SF.Application.DTOs.Protocolo;

/// <summary>
/// Input para 540-00-ATUALIZA-PROTOCOLO
/// </summary>
public class AtualizadorProtocoloInput
{
    // SF0001-PTD-ID-PTC-DSP
    public int IdProtocolo { get; set; }

    // WS01-PRECO-TOTAL-M-TOT
    public decimal PrecoTotalMercadoria { get; set; }

    // WS01-PESO-BRUTO-KG-TOT
    public decimal PesoBrutoKgTotal { get; set; }

    // WS01-PESO-LIQUIDO-KG-TOT
    public decimal PesoLiquidoKgTotal { get; set; }
}

/// <summary>
/// Output para 540-00-ATUALIZA-PROTOCOLO
/// </summary>
public class AtualizadorProtocoloOutput
{
    public bool Sucesso { get; set; }
    public int LinhasAfetadas { get; set; }
}

