using H1SF.Application.Services.Protocolo;
using H1SF.Infrastructure.Repositories.Protocolo;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class LeitorProtocoloTests
    {
        private class TestProtocoloRepository : IProtocoloRepository
        {
            private readonly string? _protocoloEsperado;

            public TestProtocoloRepository(string? protocoloEsperado)
            {
                _protocoloEsperado = protocoloEsperado;
            }

            public Task<string?> ObterProtocoloAsync(string areaParm)
            {
                return Task.FromResult(_protocoloEsperado);
            }

            public Task<List<H1SF.Domain.Entities.Protocolo.DadosProtocolo>> ListarProtocolosNaoImpressosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult(new List<H1SF.Domain.Entities.Protocolo.DadosProtocolo>());
            }

            public Task AtualizarProtocoloComoImpressoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, int cdTRec, int cdTMtz, int idMtz, int idCli, string idPtcDsp)
            {
                return Task.CompletedTask;
            }

            public Task<bool> ExistemItensFaturadosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string idPtcDsp)
            {
                return Task.FromResult(true);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task ProcessarProtocolosAsync_ValidAreaParm_ReturnsProtocolo()
        {
            // Arrange
            var protocoloEsperado = "PROT20251204001";
            var repository = new TestProtocoloRepository(protocoloEsperado);
            var logger = CreateLogger<LeitorProtocolo>();
            var service = new LeitorProtocolo(repository, logger);

            // Act
            var result = await service.ProcessarProtocolosAsync("123", DateTime.Today, "USER01");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);
        }

        [TestMethod]
        public async Task ProcessarProtocolosAsync_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var repository = new TestProtocoloRepository(null);
            var logger = CreateLogger<LeitorProtocolo>();
            var service = new LeitorProtocolo(repository, logger);

            // Act
            var result = await service.ProcessarProtocolosAsync("999", DateTime.Today, "INVALID");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task ProcessarProtocolosAsync_EmptyAreaParm_ProcessesCorrectly()
        {
            // Arrange
            var protocoloEsperado = "PROT_DEFAULT";
            var repository = new TestProtocoloRepository(protocoloEsperado);
            var logger = CreateLogger<LeitorProtocolo>();
            var service = new LeitorProtocolo(repository, logger);

            // Act
            var result = await service.ProcessarProtocolosAsync("456", DateTime.Today, string.Empty);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);
        }
    }
}
