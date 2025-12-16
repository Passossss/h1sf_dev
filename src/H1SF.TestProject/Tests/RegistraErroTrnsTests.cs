using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class RegistraErroTrnsTests
    {
        [TestMethod]
        public void Executar_QuandoChamado_NaoLancaExcecao()
        {
            // Arrange
            var service = new RegistraErroTrns();

            // Act & Assert
            service.Executar();
        }
    }
}

