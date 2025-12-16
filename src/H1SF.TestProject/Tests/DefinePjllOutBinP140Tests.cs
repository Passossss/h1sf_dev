using H1SF.Application.Services;
using H1SF.Application.Services.DefubeViasOrdenacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DefinePjllOutBinP140Tests
    {
        [TestMethod]
        public void DefinePJjlOutBinP140_ServiceCreation_Succeeds()
        {
            // Arrange
            var defineVias = new DefineViasOrdenacao();

            // Act
            var service = new DefinePJjlOutBinP140(defineVias);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Executar_WithDependency_DoesNotThrow()
        {
            // Arrange
            var defineVias = new DefineViasOrdenacao();
            var service = new DefinePJjlOutBinP140(defineVias);

            // Act
            service.Executar();

            // Assert
            Assert.IsTrue(true); // If no exception thrown, test passes
        }
    }
}
