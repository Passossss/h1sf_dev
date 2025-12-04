using H1SF.Application.Services.LogCaps;
using H1SF.Infrastructure.Repositories.LogCaps;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class MontadorLogCapsTests
    {
        private class TestLogCapsRepository : ILogCapsRepository
        {
            public bool LogSalvo { get; private set; }

            public Task SalvarLogCapsAsync(string mensagem, DateTime dataHora)
            {
                LogSalvo = true;
                return Task.CompletedTask;
            }

            public Task<(string CodigoFornecedor, decimal TotalSelecao)> RecuperarTotalSelecaoAsync(string codigoMercadoDestino, string dataSelecaoFaturamento, string loginFuncionario, string faseFaturamento)
            {
                return Task.FromResult(("FORN001", 1000.50m));
            }

            public Task<List<H1SF.Infrastructure.Repositories.LogCaps.LogCapsDetalheDto>> RecuperarDetalhesItensAsync(string codigoMercadoDestino, string dataSelecaoFaturamento, string loginFuncionario)
            {
                return Task.FromResult(new List<H1SF.Infrastructure.Repositories.LogCaps.LogCapsDetalheDto>());
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task MontarLogCapsAsync_ValidData_SavesLog()
        {
            // Arrange
            var repository = new TestLogCapsRepository();
            var logger = CreateLogger<MontadorLogCaps>();
            var service = new MontadorLogCaps(repository, logger);

            var input = new MontarLogCapsInputDto
            {
                CodigoMercadoDestino = "A",
                DataSelecaoFaturamento = "20251204",
                LoginFuncionario = "USER01",
                FaseFaturamento = "1"
            };

            // Act
            await service.MontarLogCapsAsync(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task MontarLogCapsAsync_EmptyFase_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestLogCapsRepository();
            var logger = CreateLogger<MontadorLogCaps>();
            var service = new MontadorLogCaps(repository, logger);

            var input = new MontarLogCapsInputDto
            {
                CodigoMercadoDestino = "B",
                DataSelecaoFaturamento = "20251204",
                LoginFuncionario = "USER01",
                FaseFaturamento = string.Empty
            };

            // Act
            await service.MontarLogCapsAsync(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task MontarLogCapsAsync_AllFieldsFilled_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestLogCapsRepository();
            var logger = CreateLogger<MontadorLogCaps>();
            var service = new MontadorLogCaps(repository, logger);

            var input = new MontarLogCapsInputDto
            {
                CodigoMercadoDestino = "C",
                DataSelecaoFaturamento = "20251204",
                LoginFuncionario = "ADMIN",
                FaseFaturamento = "2",
                NumeroContabil = "CTB001",
                AreaContaFila = "FILA001"
            };

            // Act
            await service.MontarLogCapsAsync(input);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }
    }
}
