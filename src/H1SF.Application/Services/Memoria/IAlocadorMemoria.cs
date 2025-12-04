namespace H1SF.Application.Services.Memoria;

/// <summary>
/// Interface para 610-00-GETMAIN-TRSC
/// Aloca memória (CICS GETMAIN)
/// </summary>
public interface IAlocadorMemoria
{
    /// <summary>
    /// 610-00-GETMAIN-TRSC SECTION
    /// EXEC CICS GETMAIN SET (WS01-ARE-IMPRESSAO) LENGTH (tamanho) END-EXEC
    /// 
    /// No CICS, GETMAIN aloca memória dinamicamente.
    /// No .NET, isso pode ser representado como alocação de buffers ou estruturas.
    /// </summary>
    /// <param name="tamanho">Tamanho da memória a ser alocada em bytes</param>
    /// <returns>Chave indicando sucesso da alocação ('S' ou 'N')</returns>
    Task<string> AlocarMemoriaAsync(int tamanho);
}
