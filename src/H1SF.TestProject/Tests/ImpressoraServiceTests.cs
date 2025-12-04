using H1SF.Application.DTO;
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
    public class ImpressoraServiceTests
    {
        private class TestDefinirImpressoraRepository : IDefinirImpressoraRepository
        {
            private readonly int? _idImpressora;
            private readonly string? _nomeImpressora;

            public TestDefinirImpressoraRepository(int? idImpressora, string? nomeImpressora)
            {
                _idImpressora = idImpressora;
                _nomeImpressora = nomeImpressora;
            }

            public Task<int?> ObterIdTipoRecolhimentoAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
            {
                return Task.FromResult<int?>(1);
            }

            public Task<SelecaoFaturamento?> ObterSelecaoFaturamentoAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
            {
                if (_idImpressora.HasValue)
                {
                    return Task.FromResult<SelecaoFaturamento?>(new SelecaoFaturamento
                    {
                        SftIdImprFtrm = _idImpressora.Value
                    });
                }
                return Task.FromResult<SelecaoFaturamento?>(null);
            }

            public Task<string?> ObterNomeImpressoraAsync(int idImpressora)
            {
                return Task.FromResult(_nomeImpressora);
            }

            public Task<bool> ValidarImpressoraExisteAsync(int idImpressora)
            {
                return Task.FromResult(_idImpressora.HasValue);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task DefinirImpressoraAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var repository = new TestDefinirImpressoraRepository(1, "IMPR001");
            var logger = CreateLogger<ImpressoraService>();
            var service = new ImpressoraService(repository, logger);

            var input = new DefinirImpressoraInputDto
            {
                CdMercDst = 123,
                DtcSelFtrm = DateTime.Today,
                LgonFunc = "USER01"
            };

            // Act
            var result = await service.DefinirImpressoraAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
            Assert.AreEqual("IMPR001", result.NomeImpressora);
        }

        [TestMethod]
        public async Task DefinirImpressoraAsync_ImpressoraNotFound_ReturnsError()
        {
            // Arrange
            var repository = new TestDefinirImpressoraRepository(null, null);
            var logger = CreateLogger<ImpressoraService>();
            var service = new ImpressoraService(repository, logger);

            var input = new DefinirImpressoraInputDto
            {
                CdMercDst = 999,
                DtcSelFtrm = DateTime.Today,
                LgonFunc = "USER99"
            };

            // Act
            var result = await service.DefinirImpressoraAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Sucesso);
            Assert.IsNotNull(result.MensagemErro);
        }

        [TestMethod]
        public async Task DefinirImpressoraAsync_ValidIdInvalidName_ReturnsError()
        {
            // Arrange
            var repository = new TestDefinirImpressoraRepository(1, null);
            var logger = CreateLogger<ImpressoraService>();
            var service = new ImpressoraService(repository, logger);

            var input = new DefinirImpressoraInputDto
            {
                CdMercDst = 123,
                DtcSelFtrm = DateTime.Today,
                LgonFunc = "USER01"
            };

            // Act
            var result = await service.DefinirImpressoraAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Sucesso);
        }
    }
}
