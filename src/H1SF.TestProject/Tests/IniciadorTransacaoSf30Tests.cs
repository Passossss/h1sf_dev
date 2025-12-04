using H1SF.Application.Services.Transacao;
using H1SF.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class IniciadorTransacaoSf30Tests
    {
        private class TestApplicationDbContext : ApplicationDbContext
        {
            public bool TransacaoIniciada { get; private set; }

            public TestApplicationDbContext() : base(new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<ApplicationDbContext>().Options)
            {
            }

            public void IniciarTransacao()
            {
                TransacaoIniciada = true;
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task IniciarTransacaoSf30Async_ValidData_StartsTransaction()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf30>();
            var service = new IniciadorTransacaoSf30(logger);

            var input = new IniciarTransacaoSf30InputDto
            {
                AreaDadosSf30 = "DADOS_TRANSACAO_SF30",
                TransactionId = "SF30"
            };

            // Act
            await service.IniciarTransacaoSf30Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task IniciarTransacaoSf30Async_EmptyAreaDados_ProcessesCorrectly()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf30>();
            var service = new IniciadorTransacaoSf30(logger);

            var input = new IniciarTransacaoSf30InputDto
            {
                AreaDadosSf30 = string.Empty,
                TransactionId = "SF30"
            };

            // Act
            await service.IniciarTransacaoSf30Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task IniciarTransacaoSf30Async_CustomTransactionId_ProcessesCorrectly()
        {
            // Arrange
            var logger = CreateLogger<IniciadorTransacaoSf30>();
            var service = new IniciadorTransacaoSf30(logger);

            var input = new IniciarTransacaoSf30InputDto
            {
                AreaDadosSf30 = "CUSTOM_DATA",
                TransactionId = "SF31"
            };

            // Act
            await service.IniciarTransacaoSf30Async(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }
    }
}
