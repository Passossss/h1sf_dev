using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class GravaMensagemMQTests
    {
        [TestMethod]
        public void GravaMensagemMQ_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new GravaMensagemMQ();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GravaMensagemMQ_Executar_DoesNotThrow()
        {
            // Arrange
            var service = new GravaMensagemMQ();

            // Act
            service.Executar();

            // Assert
            Assert.IsTrue(true); // If no exception thrown, test passes
        }

        [TestMethod]
        public void GravaMensagemMQ_ImplementsInterface_Success()
        {
            // Arrange
            var service = new GravaMensagemMQ();

            // Act & Assert
            Assert.IsInstanceOfType(service, typeof(IGravaMensagemMQ));
        }
    }
}
