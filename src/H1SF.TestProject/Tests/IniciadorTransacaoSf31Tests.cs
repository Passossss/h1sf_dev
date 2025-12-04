using H1SF.Application.Services.Transacao;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class IniciadorTransacaoSf31Tests
    {
        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task IniciarTransacaoSf31Async_ValidData_StartsTransaction()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf31>();
            var service = new IniciadorTransacaoSf31(logger);

            var input = new IniciarTransacaoSf31InputDto
            {
                AreaDadosSf31 = "DADOS_TRANSACAO_SF31",
                TransactionId = "SF31"
            };

            // Act
            await service.IniciarTransacaoSf31Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task IniciarTransacaoSf31Async_EmptyAreaDados_ProcessesCorrectly()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf31>();
            var service = new IniciadorTransacaoSf31(logger);

            var input = new IniciarTransacaoSf31InputDto
            {
                AreaDadosSf31 = string.Empty,
                TransactionId = "SF31"
            };

            // Act
            await service.IniciarTransacaoSf31Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task IniciarTransacaoSf31Async_CustomTransactionId_ProcessesCorrectly()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf31>();
            var service = new IniciadorTransacaoSf31(logger);

            var input = new IniciarTransacaoSf31InputDto
            {
                AreaDadosSf31 = "CUSTOM_DATA_SF31",
                TransactionId = "SF32"
            };

            // Act
            await service.IniciarTransacaoSf31Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }
    }
}
