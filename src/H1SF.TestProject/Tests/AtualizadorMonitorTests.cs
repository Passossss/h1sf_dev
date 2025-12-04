using H1SF.Application.Services;
using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AtualizadorMonitorTests
    {
        private class TestMonitorFaturamentoRepository : IMonitorFaturamentoRepository
        {
            private readonly MonitorFaturamento? _monitor;
            private readonly bool _cancelado;

            public TestMonitorFaturamentoRepository(MonitorFaturamento? monitor, bool cancelado = false)
            {
                _monitor = monitor;
                _cancelado = cancelado;
            }

            public Task<MonitorFaturamento?> ObterMonitorAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult(_monitor);
            }

            public Task AtualizarFaseAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc, DateTime dataFase, int fase)
            {
                return Task.CompletedTask;
            }

            public Task<bool> VerificarCancelamentoAsync(char cdMercDst, string dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult(_cancelado);
            }

            public Task AtualizarFaseLbrcImpsAsync(string codigoMercadoDestino, string dataSelecaoFaturamento, string loginFuncionario)
            {
                return Task.CompletedTask;
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task AtualizarMonitorAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            var monitor = new MonitorFaturamento
            {
                CodigoMercadoriaDestino = 'A',
                TimestampSelecao = "20251204",
                LoginFuncionario = "USER01"
            };
            var repository = new TestMonitorFaturamentoRepository(monitor, cancelado: false);
            var logger = CreateLogger<AtualizadorMonitor>();
            var service = new AtualizadorMonitor(repository, logger);

            var parametros = new FaturamentoParametros
            {
                CodigoMercadoriaDestino = 'A',
                DataHoraSelecao = "20251204",
                LoginFuncionario = "USER01",
                FaseFaturamento = '1'
            };

            // Act
            await service.AtualizarMonitorAsync(parametros);

            // Assert
            Assert.IsFalse(service.FoiCancelado);
        }

        [TestMethod]
        public async Task AtualizarMonitorAsync_ProcessoCancelado_SetsCancelFlag()
        {
            // Arrange
            var monitor = new MonitorFaturamento
            {
                CodigoMercadoriaDestino = 'A',
                TimestampSelecao = "20251204",
                LoginFuncionario = "USER01"
            };
            var repository = new TestMonitorFaturamentoRepository(monitor, cancelado: true);
            var logger = CreateLogger<AtualizadorMonitor>();
            var service = new AtualizadorMonitor(repository, logger);

            var parametros = new FaturamentoParametros
            {
                CodigoMercadoriaDestino = 'A',
                DataHoraSelecao = "20251204",
                LoginFuncionario = "USER01",
                FaseFaturamento = '1'
            };

            // Act
            await service.AtualizarMonitorAsync(parametros);

            // Assert
            Assert.IsTrue(service.FoiCancelado);
        }

        [TestMethod]
        public async Task AtualizarMonitorAsync_MonitorNotFound_ProcessesWithoutError()
        {
            // Arrange
            var repository = new TestMonitorFaturamentoRepository(null, cancelado: false);
            var logger = CreateLogger<AtualizadorMonitor>();
            var service = new AtualizadorMonitor(repository, logger);

            var parametros = new FaturamentoParametros
            {
                CodigoMercadoriaDestino = 'Z',
                DataHoraSelecao = "20251204",
                LoginFuncionario = "USER99",
                FaseFaturamento = '1'
            };

            // Act
            await service.AtualizarMonitorAsync(parametros);

            // Assert
            Assert.IsFalse(service.FoiCancelado);
        }
    }
}
