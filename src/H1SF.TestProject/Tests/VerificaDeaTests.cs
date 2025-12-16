using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class VerificaDeaTests
    {
        [TestMethod]
        public void Executar_QuandoChamado_NaoLancaExcecao()
        {
            // Arrange
            var service = new VerificaDea();

            // Act & Assert
            service.Executar();
        }
    }
}


