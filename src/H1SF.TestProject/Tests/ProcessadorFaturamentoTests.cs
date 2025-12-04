using H1SF.Application.Services;
using H1SF.Domain.Entities.Faturamento;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class ProcessadorFaturamentoTests
    {
        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public void RetrieveParametro_Valid24ByteInput_ReturnsParameters()
        {
            // Arrange
            var logger = CreateLogger<ProcessadorFaturamento>();
            var service = new ProcessadorFaturamento(logger);
            var input = "A20251204123456USER01  1"; // 24 bytes

            // Act
            var result = service.RetrieveParametro(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual('A', result.CodigoMercadoriaDestino);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RetrieveParametro_InvalidLength_ThrowsException()
        {
            // Arrange
            var logger = CreateLogger<ProcessadorFaturamento>();
            var service = new ProcessadorFaturamento(logger);
            var input = "SHORT"; // Less than 24 bytes

            // Act
            service.RetrieveParametro(input);

            // Assert: Exception is expected
        }

        [TestMethod]
        public void RetrieveParametro_ExactlyLongInput_ProcessesCorrectly()
        {
            // Arrange
            var logger = CreateLogger<ProcessadorFaturamento>();
            var service = new ProcessadorFaturamento(logger);
            var input = "B20251204123456ADMIN012"; // Exactly 24 bytes

            // Act
            var result = service.RetrieveParametro(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual('B', result.CodigoMercadoriaDestino);
        }
    }
}
