using H1SF.Application.Services.EmiteLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class EmiteLinkTests
    {
        [TestMethod]
        public void EmiteLinkH1SFService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new EmiteLinkH1SFService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Executar_ThrowsNotImplemented_AsExpected()
        {
            // Arrange
            var service = new EmiteLinkH1SFService();

            // Act & Assert
            Assert.ThrowsException<System.NotImplementedException>(() => service.Executar());
        }
    }
}
