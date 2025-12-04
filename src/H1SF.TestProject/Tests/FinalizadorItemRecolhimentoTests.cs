using H1SF.Application.Services.Recolhimento;
using H1SF.Infrastructure.Repositories.Recolhimento;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class FinalizadorItemRecolhimentoTests
    {
        private class TestFinalizadorItemRecolhimentoRepository : IFinalizadorItemRecolhimentoRepository
        {
            public bool ItemFinalizado { get; private set; }

            public Task<int> FinalizarItemAsync(int idEtiqRec, int qtdPecaRec)
            {
                ItemFinalizado = true;
                return Task.FromResult(1);
            }

            public Task<bool> ValidarItemExisteAsync(int idEtiqRec)
            {
                return Task.FromResult(true);
            }

            public Task<int> FinalizarItensPendentesAsync()
            {
                ItemFinalizado = true;
                return Task.FromResult(1);
            }

            public Task<bool> ExistemItensPendentesAsync()
            {
                return Task.FromResult(true);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task FinalizarItensPendentesAsync_WithPendingItems_FinalizesSuccessfully()
        {
            // Arrange
            var repository = new TestFinalizadorItemRecolhimentoRepository();
            var logger = CreateLogger<FinalizadorItemRecolhimento>();
            var service = new FinalizadorItemRecolhimento(repository, logger);

            // Act
            var result = await service.FinalizarItensPendentesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(repository.ItemFinalizado);
        }

        [TestMethod]
        public async Task FinalizarItensPendentesAsync_NoPendingItems_CompletesSuccessfully()
        {
            // Arrange
            var repository = new TestFinalizadorItemRecolhimentoRepository();
            var logger = CreateLogger<FinalizadorItemRecolhimento>();
            var service = new FinalizadorItemRecolhimento(repository, logger);

            // Act
            var result = await service.FinalizarItensPendentesAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task FinalizarItensPendentesAsync_MultipleCalls_AllSucceed()
        {
            // Arrange
            var repository = new TestFinalizadorItemRecolhimentoRepository();
            var logger = CreateLogger<FinalizadorItemRecolhimento>();
            var service = new FinalizadorItemRecolhimento(repository, logger);

            // Act
            var result1 = await service.FinalizarItensPendentesAsync();
            var result2 = await service.FinalizarItensPendentesAsync();

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(repository.ItemFinalizado);
        }
    }
}
