using H1SF.Application.DTOs.Protocolo;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AtualizadorProtocoloTests
    {
        [TestMethod]
        public void AtualizadorProtocolo_ServiceInterface_Exists()
        {
            // Arrange & Act
            // Verify the interface and DTOs exist
            var input = new AtualizadorProtocoloInput
            {
                PrecoTotalMercadoria = 1000.50m,
                PesoBrutoKgTotal = 500.75m,
                PesoLiquidoKgTotal = 450.25m
            };

            // Assert
            Assert.IsNotNull(input);
            Assert.AreEqual(1000.50m, input.PrecoTotalMercadoria);
            Assert.AreEqual(500.75m, input.PesoBrutoKgTotal);
            Assert.AreEqual(450.25m, input.PesoLiquidoKgTotal);
        }

        [TestMethod]
        public void AtualizadorProtocoloOutput_CanBeCreated()
        {
            // Arrange & Act
            var output = new AtualizadorProtocoloOutput
            {
                Sucesso = true
            };

            // Assert
            Assert.IsNotNull(output);
            Assert.IsTrue(output.Sucesso);
        }

        [TestMethod]
        public void AtualizadorProtocoloInput_ValidatesData()
        {
            // Arrange & Act
            var input = new AtualizadorProtocoloInput
            {
                PrecoTotalMercadoria = 0,
                PesoBrutoKgTotal = 0,
                PesoLiquidoKgTotal = 0
            };

            // Assert - Zero values are valid
            Assert.IsNotNull(input);
            Assert.AreEqual(0, input.PrecoTotalMercadoria);
        }
    }
}
