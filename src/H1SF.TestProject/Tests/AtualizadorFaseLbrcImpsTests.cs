using H1SF.Application.Services.Monitor;
using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AtualizadorFaseLbrcImpsTests
    {
        private class TestMonitorFaturamentoRepository : IMonitorFaturamentoRepository
        {
            public bool FaseAtualizada { get; private set; }

            public Task<MonitorFaturamento?> ObterMonitorAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult<MonitorFaturamento?>(null);
            }

            public Task AtualizarFaseAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc, DateTime dtcFase, int fase)
            {
                return Task.CompletedTask;
            }

            public Task<bool> VerificarCancelamentoAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult(false);
            }

            public Task AtualizarFaseLbrcImpsAsync(string codigoMercadoDestino, string dataSelecaoFaturamento, string loginFuncionario)
            {
                FaseAtualizada = true;
                return Task.CompletedTask;
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task AtualizarFaseLbrcImpsAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            var repository = new TestMonitorFaturamentoRepository();
            var logger = CreateLogger<AtualizadorFaseLbrcImps>();
            var service = new AtualizadorFaseLbrcImps(repository, logger);

            var input = new AtualizarFaseLbrcImpsInputDto
            {
                CodigoMercadoDestino = "A",
                DataSelecaoFaturamento = "20251204",
                LoginFuncionario = "USER01"
            };

            // Act
            await service.AtualizarFaseLbrcImpsAsync(input);

            // Assert
            Assert.IsTrue(repository.FaseAtualizada);
        }

        [TestMethod]
        public async Task AtualizarFaseLbrcImpsAsync_EmptyLoginFuncionario_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestMonitorFaturamentoRepository();
            var logger = CreateLogger<AtualizadorFaseLbrcImps>();
            var service = new AtualizadorFaseLbrcImps(repository, logger);

            var input = new AtualizarFaseLbrcImpsInputDto
            {
                CodigoMercadoDestino = "B",
                DataSelecaoFaturamento = "20251204",
                LoginFuncionario = string.Empty
            };

            // Act
            await service.AtualizarFaseLbrcImpsAsync(input);

            // Assert
            Assert.IsTrue(repository.FaseAtualizada);
        }

        [TestMethod]
        public async Task AtualizarFaseLbrcImpsAsync_DifferentCodigo_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestMonitorFaturamentoRepository();
            var logger = CreateLogger<AtualizadorFaseLbrcImps>();
            var service = new AtualizadorFaseLbrcImps(repository, logger);

            var input = new AtualizarFaseLbrcImpsInputDto
            {
                CodigoMercadoDestino = "C",
                DataSelecaoFaturamento = "20251203",
                LoginFuncionario = "ADMIN"
            };

            // Act
            await service.AtualizarFaseLbrcImpsAsync(input);

            // Assert
            Assert.IsTrue(repository.FaseAtualizada);
        }
    }
}
