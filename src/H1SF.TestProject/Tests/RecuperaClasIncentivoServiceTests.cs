using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class RecuperaClasIncentivoServiceTests
    {
        private class TestRegistraErroTrnsService : RecuperaClasIncentivoService.IRegistraErroTrnsService
        {
            public bool ErroRegistrado { get; private set; }
            public string UltimaMensagemErro { get; private set; } = string.Empty;
            public string UltimoCodigoErro { get; private set; } = string.Empty;

            public void RegistrarErro(string mensagem, string codigo)
            {
                ErroRegistrado = true;
                UltimaMensagemErro = mensagem;
                UltimoCodigoErro = codigo;
            }
        }

        [TestMethod]
        public void RecuperarClasIncentivo_QuandoChamado_NaoLancaExcecao()
        {
            // Arrange
            var registraErro = new TestRegistraErroTrnsService();
            var service = new RecuperaClasIncentivoService(registraErro);

            // Act & Assert
            service.RecuperarClasIncentivo();
        }

        [TestMethod]
        public void RecuperarClasIncentivo_QuandoChamado_ExecutaComSucesso()
        {
            // Arrange
            var registraErro = new TestRegistraErroTrnsService();
            var service = new RecuperaClasIncentivoService(registraErro);

            // Act
            service.RecuperarClasIncentivo();

            // Assert
            // O método executa sem lançar exceção
            Assert.IsNotNull(service);
        }
    }
}

