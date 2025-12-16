using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DescargaDanfeTests
    {
        [TestMethod]
        public void DanfeService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new DanfeService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void DescarregaViasDanfe_CallMethod_DoesNotThrow()
        {
            // Arrange
            var service = new DanfeService();

            // Act
            service.DescarregaViasDanfe();

            // Assert
            Assert.IsTrue(true); // If no exception thrown, test passes
        }

        [TestMethod]
        public void DanfeService_ImplementsInterface_Success()
        {
            // Arrange
            var service = new DanfeService();

            // Act & Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IDanfeService));
        }
    }
}
