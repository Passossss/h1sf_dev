using H1SF.Domain.Entities.Errors;
using H1SF.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace H1SF.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de erros
/// </summary>
public class ServicoErros : IServicoErros
{
    private readonly ILogger<ServicoErros> _logger;
    
    public ServicoErros(ILogger<ServicoErros> logger)
    {
        _logger = logger;
    }
    
    public void RegistrarErro(ErroTransacional erro)
    {
        // Log estruturado do erro
        _logger.LogError(
            "Erro Transacional - Programa: {Programa}, Severidade: {Severidade}, Mensagem: {Mensagem}, Dados: {Dados}, DataHora: {DataHora}",
            erro.Programa,
            erro.Severidade,
            erro.Mensagem,
            erro.DadosOriginais,
            erro.DataHora);

    }
}
