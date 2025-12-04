namespace H1SF.Application.Services.Memoria;

/// <summary>
/// Interface para liberação de memória (FREEMAIN)
/// 615-00-FREEMAIN-TRSC SECTION
/// Autor: A.C.ANDREATTA
/// </summary>
public interface ILiberadorMemoria
{
    /// <summary>
    /// 615-00-FREEMAIN-TRSC
    /// EXEC CICS FREEMAIN DATA (WS01-ARE-IMPRESSAO) END-EXEC
    /// 
    /// No CICS, FREEMAIN libera memória alocada com GETMAIN.
    /// Em .NET, o gerenciamento de memória é automático (GC),
    /// mas mantemos a interface para compatibilidade com a lógica COBOL.
    /// </summary>
    /// <param name="chaveGetmain">Chave indicando se houve GETMAIN anterior ('S' ou 'N')</param>
    Task LiberarMemoriaAsync(string chaveGetmain);
}
