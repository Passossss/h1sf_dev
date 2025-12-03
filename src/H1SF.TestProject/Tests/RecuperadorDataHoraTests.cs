using H1SF.Application.DTOs.DataHora;
using H1SF.Application.Services.DataHora;
using H1SF.Infrastructure.Repositories.DataHora;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class RecuperadorDataHoraTests
    {
        private class TestDataHoraRepository : IDataHoraRepository
        {
            private readonly string _dataHora;

            public TestDataHoraRepository(string dataHora)
            {
                _dataHora = dataHora;
            }

            public Task<string> ObterDataHoraSistemaAsync()
            {
                return Task.FromResult(_dataHora);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task RecuperarDataHoraAsync_ValidDataHora_ReturnsCorrectDto()
        {
            // Arrange
            var validDataHora = "20251203153045"; // Example: 03/12/2025 15:30:45
            var repository = new TestDataHoraRepository(validDataHora);
            var logger = CreateLogger<RecuperadorDataHora>();
            var service = new RecuperadorDataHora(repository, logger);

            // Act
            var result = await service.RecuperarDataHoraAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validDataHora, result.DataHoraFormatada);
            Assert.AreEqual(new DateTime(2025, 12, 3, 15, 30, 45), result.DataHora);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RecuperarDataHoraAsync_EmptyDataHora_ThrowsInvalidOperationException()
        {
            // Arrange
            var repository = new TestDataHoraRepository(string.Empty);
            var logger = CreateLogger<RecuperadorDataHora>();
            var service = new RecuperadorDataHora(repository, logger);

            // Act
            await service.RecuperarDataHoraAsync();

            // Assert: Exception is expected
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task RecuperarDataHoraAsync_InvalidDataHoraFormat_ThrowsFormatException()
        {
            // Arrange
            var invalidDataHora = "INVALID_DATE";
            var repository = new TestDataHoraRepository(invalidDataHora);
            var logger = CreateLogger<RecuperadorDataHora>();
            var service = new RecuperadorDataHora(repository, logger);

            // Act
            await service.RecuperarDataHoraAsync();

            // Assert: Exception is expected
        }
    }
}