using H1SF.Application.Services.InstrucaoEspecial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InstrucaoEspecialTests
    {
        [TestMethod]
        public void InstrucaoEspecialService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new InstrucaoEspecialService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void InstrucaoEspecialService_ImplementsInterface_Success()
        {
            // Arrange
            var service = new InstrucaoEspecialService();

            // Act & Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IInstrucaoEspecialService));
        }
    }
}
