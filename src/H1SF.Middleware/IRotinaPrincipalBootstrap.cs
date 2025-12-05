
using H1SF.Application.Services;

namespace H1SF.Middleware
{
    public interface IRotinaPrincipalBootstrap
    {
        void Dispose();
        void RotinaPrincipal();
        void ProcessamentoLogico();
    }
}