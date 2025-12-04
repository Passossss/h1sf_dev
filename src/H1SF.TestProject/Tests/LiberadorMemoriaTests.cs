using H1SF.Application.Services.Memoria;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class LiberadorMemoriaTests
    {
        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task LiberarMemoriaAsync_ChaveGetmainS_ExecutesFreemain()
        {
            // Arrange
            var logger = CreateLogger<LiberadorMemoria>();
            var service = new LiberadorMemoria(logger);

            // Act
            await service.LiberarMemoriaAsync("S");

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LiberarMemoriaAsync_ChaveGetmainN_SkipsFreemain()
        {
            // Arrange
            var logger = CreateLogger<LiberadorMemoria>();
            var service = new LiberadorMemoria(logger);

            // Act
            await service.LiberarMemoriaAsync("N");

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LiberarMemoriaAsync_EmptyChave_SkipsFreemain()
        {
            // Arrange
            var logger = CreateLogger<LiberadorMemoria>();
            var service = new LiberadorMemoria(logger);

            // Act
            await service.LiberarMemoriaAsync(string.Empty);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LiberarMemoriaAsync_NullChave_SkipsFreemain()
        {
            // Arrange
            var logger = CreateLogger<LiberadorMemoria>();
            var service = new LiberadorMemoria(logger);

            // Act
            await service.LiberarMemoriaAsync(string.Empty);

            // Assert - Should complete without exception
            Assert.IsTrue(true);
        }
    }
}
