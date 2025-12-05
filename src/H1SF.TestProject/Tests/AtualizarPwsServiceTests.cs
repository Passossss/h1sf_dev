using H1SF.Application.Services.FaturamentoPws;
using H1SF.Infrastructure.Repositories.FaturamentoPws;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class AtualizarPwsServiceTests
    {
        private class TestAtualizarPwsRepository : IAtualizarPwsRepository
        {
            public bool AtualizacaoRealizada { get; private set; }

            public Task AtualizarPwsAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string areaParm)
            {
                AtualizacaoRealizada = true;
                return Task.CompletedTask;
            }

            public Task<bool> DeveExecutarAtualizacaoAsync(string? sftIcNaczIcpnBt, string? sftCdTRec)
            {
                return Task.FromResult(true);
            }

            public Task<List<ItemFaturadoAgrupadoDto>> ObterItensAgrupadosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm)
            {
                return Task.FromResult(new List<ItemFaturadoAgrupadoDto>());
            }

            public Task<bool> AtualizarItemRecolhimentoAsync(int idEtiqRec, int qPecaRec)
            {
                return Task.FromResult(true);
            }

            public Task<int?> ObterSomaQuantidadesItemAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm, int idEtiqRec)
            {
                return Task.FromResult<int?>(0);
            }

            public Task<List<VolumeInfoDto>> ObterVolumesDistintosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm)
            {
                return Task.FromResult(new List<VolumeInfoDto>());
            }

            public Task<bool> AtualizarIdDocumentoFiscalAsync(int idVol, string idNf)
            {
                return Task.FromResult(true);
            }

            public Task<bool> AtualizarVolumeFaturadoAsync(int idVol)
            {
                return Task.FromResult(true);
            }

            public Task<bool> VerificarTodosItensFinalizadosAsync(int idVol)
            {
                return Task.FromResult(true);
            }
        }

        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task AtualizarPwsAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            var repository = new TestAtualizarPwsRepository();
            var logger = CreateLogger<AtualizarPwsService>();
            var service = new AtualizarPwsService(repository, logger);

            var input = new AtualizarPwsInputDto
            {
                CdMercDst = "123",
                DtcSelFtrm = DateTime.Today,
                LgonFunc = "USER01",
                FaseFtrm = "1"
            };

            // Act
            var result = await service.ExecutarAtualizacaoPwsAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
        }

        [TestMethod]
        public async Task AtualizarPwsAsync_ZeroCdMercDst_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestAtualizarPwsRepository();
            var logger = CreateLogger<AtualizarPwsService>();
            var service = new AtualizarPwsService(repository, logger);

            var input = new AtualizarPwsInputDto
            {
                CdMercDst = "0",
                DtcSelFtrm = DateTime.Today,
                LgonFunc = "USER01",
                FaseFtrm = "1"
            };

            // Act
            var result = await service.ExecutarAtualizacaoPwsAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
        }

        [TestMethod]
        public async Task AtualizarPwsAsync_EmptyLgonFunc_ProcessesCorrectly()
        {
            // Arrange
            var repository = new TestAtualizarPwsRepository();
            var logger = CreateLogger<AtualizarPwsService>();
            var service = new AtualizarPwsService(repository, logger);

            var input = new AtualizarPwsInputDto
            {
                CdMercDst = "456",
                DtcSelFtrm = DateTime.Today,
                LgonFunc = string.Empty,
                FaseFtrm = "1"
            };

            // Act
            var result = await service.ExecutarAtualizacaoPwsAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Sucesso);
        }
    }
}
