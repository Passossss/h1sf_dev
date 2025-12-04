using H1SF.Application.Services.Memoria;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AlocadorMemoriaTests
    {
        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task AlocarMemoriaAsync_ValidSize_ReturnsSuccess()
        {
            // Arrange
            var logger = CreateLogger<AlocadorMemoria>();
            var service = new AlocadorMemoria(logger);

            // Act
            var result = await service.AlocarMemoriaAsync(1024);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("S", result);
        }

        [TestMethod]
        public async Task AlocarMemoriaAsync_ZeroSize_ReturnsFailure()
        {
            // Arrange
            var logger = CreateLogger<AlocadorMemoria>();
            var service = new AlocadorMemoria(logger);

            // Act
            var result = await service.AlocarMemoriaAsync(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("N", result);
        }

        [TestMethod]
        public async Task AlocarMemoriaAsync_NegativeSize_ReturnsFailure()
        {
            // Arrange
            var logger = CreateLogger<AlocadorMemoria>();
            var service = new AlocadorMemoria(logger);

            // Act
            var result = await service.AlocarMemoriaAsync(-100);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("N", result);
        }
    }
}
