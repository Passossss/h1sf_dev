using H1SF.Domain.Entities.Errors;

namespace H1SF.Domain.Interfaces;

/// <summary>
/// Interface para serviço de erros
/// </summary>
public interface IServicoErros
{
    void RegistrarErro(ErroTransacional erro);
}
