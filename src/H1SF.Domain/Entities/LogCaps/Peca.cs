namespace H1SF.Domain.Entities.LogCaps;

/// <summary>
/// Entidade PECA (H1SR schema) - Nome da peça em inglês
/// </summary>
public class Peca
{
    public string IdPeca { get; set; } = string.Empty; // ID_PECA
    public string NomePecaIngles { get; set; } = string.Empty; // NM_PECA_ING
}
