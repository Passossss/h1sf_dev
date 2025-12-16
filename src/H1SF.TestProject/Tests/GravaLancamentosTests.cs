using H1SF.Application.Services.GravaLançamentos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class GravaLancamentosTests
    {
        [TestMethod]
        public void GravaLancamentosCtService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new GravaLancamentosCtService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GravaLancamentosCtService_ImplementsInterface_Success()
        {
            // Arrange
            var service = new GravaLancamentosCtService();

            // Act & Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IGravaLancamentosCtService));
        }
    }
}
