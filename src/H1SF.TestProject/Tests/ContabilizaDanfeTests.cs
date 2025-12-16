using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class ContabilizaDanfeTests
    {
        [TestMethod]
        public void ContabilizacaoService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new ContabilizacaoService("test-connection-string");

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ContabilizacaoService_HasValidStructure_Success()
        {
            // Arrange
            var service = new ContabilizacaoService("test-connection-string");

            // Act & Assert
            Assert.IsNotNull(service);
            // Verify service can be instantiated
            Assert.IsInstanceOfType(service, typeof(IContabilizacaoService));
        }
    }
}
