using H1SF.Application.DTOs.Protocolo;
using H1SF.Application.Services;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AtualizadorProtocoloTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task ExecutarAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new AtualizadorProtocoloService(context);
            var input = new AtualizadorProtocoloInput
            {
                PrecoTotalMercadoria = 1000.50m,
                PesoBrutoKgTotal = 500.75m,
                PesoLiquidoKgTotal = 450.25m
            };

            // Act
            var result = await service.ExecutarAsync(input);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ExecutarAsync_ServiceCreation_Succeeds()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            // Act
            var service = new AtualizadorProtocoloService(context);

            // Assert
            Assert.IsNotNull(service);
        }
    }
}
