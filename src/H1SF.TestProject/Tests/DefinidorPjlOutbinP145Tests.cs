using H1SF.Application.DTOs.DefinidorPjl;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DefinidorPjlOutbinP145Tests
    {
        [TestMethod]
        public void DefinidorPjlOutbinP145Service_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new DefinidorPjlOutbinP145Service();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public async Task ExecutarAsync_ValidInput_ReturnsOutput()
        {
            // Arrange
            var service = new DefinidorPjlOutbinP145Service();
            var input = new DefinidorPjlOutbinP145Input
            {
                QuantidadeViasPackList = 3,
                CodigoMercadoriaDestino = "E",
                CodigoModalidadeTransporte = "6"
            };

            // Act
            var result = await service.ExecutarAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinidorPjlOutbinP145Output));
        }

        [TestMethod]
        public async Task ExecutarAsync_ZeroQuantidadeVias_ReturnsSuccess()
        {
            // Arrange
            var service = new DefinidorPjlOutbinP145Service();
            var input = new DefinidorPjlOutbinP145Input
            {
                QuantidadeViasPackList = 0,
                CodigoMercadoriaDestino = "E",
                CodigoModalidadeTransporte = "6"
            };

            // Act
            var result = await service.ExecutarAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
        }
    }
}
