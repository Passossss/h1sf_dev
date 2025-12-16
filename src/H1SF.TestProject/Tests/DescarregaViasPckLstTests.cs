using H1SF.Application.DTOs.DescarregaVias;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DescarregaViasPckLstTests
    {
        [TestMethod]
        public void DescarregaViasPckLstService_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new DescarregaViasPckLstService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public async Task ExecutarAsync_ValidInput_ReturnsOutput()
        {
            // Arrange
            var service = new DescarregaViasPckLstService();
            var input = new DescarregaViasPckLstInput
            {
                NumeroViasVolume = 2,
                IndiceImpressao = 3
            };

            // Act
            var result = await service.ExecutarAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DescarregaViasPckLstOutput));
        }

        [TestMethod]
        public async Task ExecutarAsync_ZeroIndiceImpressao_ReturnsOutput()
        {
            // Arrange
            var service = new DescarregaViasPckLstService();
            var input = new DescarregaViasPckLstInput
            {
                NumeroViasVolume = 0,
                IndiceImpressao = 0
            };

            // Act
            var result = await service.ExecutarAsync(input);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
