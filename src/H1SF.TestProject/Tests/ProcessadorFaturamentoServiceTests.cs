using H1SF.Application.Services;
using H1SF.Domain.Entities.Faturamento;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class ProcessadorFaturamentoServiceTests
    {
        private class TestProcessadorFaturamento : IProcessadorFaturamento
        {
            public FaturamentoParametros RetrieveParametro(string parametrosEntrada)
            {
                return new FaturamentoParametros
                {
                    CodigoMercadoriaDestino = 'A',
                    DataHoraSelecao = "20251204123456",
                    LoginFuncionario = "USER01",
                    FaseFaturamento = '1'
                };
            }
        }

        private class TestAtualizadorMonitor : IAtualizadorMonitor
        {
            public bool FoiCancelado => false;
            
            public Task AtualizarMonitorAsync(FaturamentoParametros parametros)
            {
                return Task.CompletedTask;
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task ProcessarFaturamentoAsync_ValidInput_ProcessesSuccessfully()
        {
            // Arrange
            var processador = new TestProcessadorFaturamento();
            var atualizador = new TestAtualizadorMonitor();
            var logger = CreateLogger<ProcessadorFaturamentoService>();
            var service = new ProcessadorFaturamentoService(processador, atualizador, logger);
            var input = "A20251204123456USER01  1"; // 24 bytes

            // Act
            var result = await service.ProcessarFaturamentoAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual('A', result.CodigoMercadoriaDestino);
        }

        [TestMethod]
        public async Task ProcessarFaturamentoAsync_DifferentMercadoria_ProcessesCorrectly()
        {
            // Arrange
            var processador = new TestProcessadorFaturamento();
            var atualizador = new TestAtualizadorMonitor();
            var logger = CreateLogger<ProcessadorFaturamentoService>();
            var service = new ProcessadorFaturamentoService(processador, atualizador, logger);
            var input = "D20251204098765ADMIN   2"; // 24 bytes - Mercadoria D, Fase 2

            // Act
            var result = await service.ProcessarFaturamentoAsync(input);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ProcessarFaturamentoAsync_EmptyUser_ProcessesCorrectly()
        {
            // Arrange
            var processador = new TestProcessadorFaturamento();
            var atualizador = new TestAtualizadorMonitor();
            var logger = CreateLogger<ProcessadorFaturamentoService>();
            var service = new ProcessadorFaturamentoService(processador, atualizador, logger);
            var input = "C20251204111111        1"; // 24 bytes - Empty user

            // Act
            var result = await service.ProcessarFaturamentoAsync(input);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
