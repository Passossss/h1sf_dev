using H1SF.Application.Services.Fabrica;
using H1SF.Domain.Entities.Fabrica;
using H1SF.Infrastructure.Repositories.Fabrica;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class RecuperadorCnpjFabricaTests
    {
        private class TestCnpjFabricaRepository : ICnpjFabricaRepository
        {
            private readonly CnpjFabrica? _cnpjEsperado;

            public TestCnpjFabricaRepository(CnpjFabrica? cnpjEsperado)
            {
                _cnpjEsperado = cnpjEsperado;
            }

            public Task<CnpjFabrica?> ObterCnpjVendaAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult(_cnpjEsperado);
            }

            public Task<CnpjFabrica?> ObterCnpjTriangulacaoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string icSim)
            {
                return Task.FromResult(_cnpjEsperado);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task RecuperarCnpjAsync_VendaNormal_ReturnsCnpj()
        {
            // Arrange
            var cnpjEsperado = new CnpjFabrica
            {
                IdCnpj = "12345678000190",
                CdTPrd = "P001",
                CdFbr = 1
            };
            var repository = new TestCnpjFabricaRepository(cnpjEsperado);
            var logger = CreateLogger<RecuperadorCnpjFabrica>();
            var service = new RecuperadorCnpjFabrica(repository, logger);

            // Act
            var result = await service.RecuperarCnpjAsync("A", "1", 123, DateTime.Today, "USER01", "S");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("12345678000190", result.IdCnpj);
        }

        [TestMethod]
        public async Task RecuperarCnpjAsync_Triangulacao_ReturnsCnpj()
        {
            // Arrange
            var cnpjEsperado = new CnpjFabrica
            {
                IdCnpj = "98765432000100",
                CdTPrd = "P002",
                CdFbr = 2
            };
            var repository = new TestCnpjFabricaRepository(cnpjEsperado);
            var logger = CreateLogger<RecuperadorCnpjFabrica>();
            var service = new RecuperadorCnpjFabrica(repository, logger);

            // Act - cdMercDst='D' e faseFtrm='2' = triangulação
            var result = await service.RecuperarCnpjAsync("D", "2", 456, DateTime.Today, "USER02", "S");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("98765432000100", result.IdCnpj);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RecuperarCnpjAsync_CnpjNotFound_ThrowsException()
        {
            // Arrange
            var repository = new TestCnpjFabricaRepository(null); // Passing null causes the exception
            var logger = CreateLogger<RecuperadorCnpjFabrica>();
            var service = new RecuperadorCnpjFabrica(repository, logger);

            // Act - Should throw InvalidOperationException
            await service.RecuperarCnpjAsync("A", "1", 1, DateTime.Now, "USER", "N");
        }
    }
}
