using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Memoria;

/// <summary>
/// 615-00-FREEMAIN-TRSC - Libera memória alocada dinamicamente
/// Linhas COBOL: aproximadamente 7500-7520
/// Autor: A.C.ANDREATTA
/// </summary>
public class LiberadorMemoria : ILiberadorMemoria
{
    private readonly ILogger<LiberadorMemoria> _logger;

    public LiberadorMemoria(ILogger<LiberadorMemoria> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 615-00-FREEMAIN-TRSC SECTION
    /// 
    /// COBOL:
    /// IF WS31-CHV-GETMAIN NOT EQUAL 'S'
    ///     GO TO 615-99-EXIT.
    /// EXEC CICS FREEMAIN DATA (WS01-ARE-IMPRESSAO) END-EXEC.
    /// 
    /// No ambiente CICS mainframe, FREEMAIN:
    /// - Libera memória previamente alocada com GETMAIN
    /// - Previne memory leaks em ambiente transacional
    /// - É crítico para gerenciamento de recursos do sistema
    /// 
    /// Em .NET:
    /// - Garbage Collector gerencia memória automaticamente
    /// - Podemos usar GC.Collect() para sugerir coleta imediata
    /// - Mais útil para grandes objetos ou recursos não gerenciados
    /// - Mantemos a lógica para compatibilidade com fluxo COBOL
    /// </summary>
    /// <param name="chaveGetmain">
    /// Chave indicando se houve GETMAIN anterior:
    /// 'S' = Sim, houve alocação (executa FREEMAIN)
    /// 'N' ou outro = Não houve alocação (pula FREEMAIN)
    /// </param>
    public async Task LiberarMemoriaAsync(string chaveGetmain)
    {
        _logger.LogDebug("615-00-FREEMAIN-TRSC iniciado");
        _logger.LogDebug("Chave GETMAIN: {ChaveGetmain}", chaveGetmain);

        try
        {
            // IF WS31-CHV-GETMAIN NOT EQUAL 'S'
            //     GO TO 615-99-EXIT
            if (string.IsNullOrEmpty(chaveGetmain) || !chaveGetmain.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("FREEMAIN não executado - chave GETMAIN não é 'S'");
                return; // GO TO 615-99-EXIT
            }

            _logger.LogInformation("FREEMAIN: Liberando memória alocada");

            // EXEC CICS FREEMAIN DATA (WS01-ARE-IMPRESSAO) END-EXEC
            // Em .NET, o GC gerencia automaticamente, mas podemos sugerir coleta
            // para grandes objetos ou quando necessário liberar recursos imediatamente
            
            // Força coleta de gerações 0 e 1 (objetos temporários)
            // Geração 2 (objetos de longa duração) é coletada apenas se necessário
            GC.Collect(1, GCCollectionMode.Optimized, blocking: false);
            
            _logger.LogInformation("FREEMAIN: Memória liberada - coleta de garbage sugerida");
            _logger.LogDebug("615-00-FREEMAIN-TRSC concluído com sucesso");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar FREEMAIN");
            throw;
        }
    }
}
