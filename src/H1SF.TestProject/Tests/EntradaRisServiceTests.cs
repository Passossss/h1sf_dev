using H1SF.Application.DTOs.EntradaNfIcRis;
using H1SF.Application.Services.EntradaNfIcRis;
using H1SF.Domain.Entities.EntradaNfIcRis;
using H1SF.Infrastructure.Repositories.EntradaNfIcRis;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class EntradaRisServiceTests
    {
        private class TestEntradaRisRepository : IEntradaRisRepository
        {
            public EntradaRisRequest? LastRequest { get; private set; }
            public EntradaRisResponse? MockResponse { get; set; }

            public Task<Guid> SalvarRequisicaoAsync(EntradaRisRequest request)
            {
                LastRequest = request;
                return Task.FromResult(Guid.NewGuid());
            }

            public Task AtualizarRespostaAsync(int id, EntradaRisResponse response)
            {
                return Task.CompletedTask;
            }

            public Task<EntradaRisResponse> EmitirLinkParaInterfaceRisAsync(EntradaRisRequest request)
            {
                return Task.FromResult(MockResponse ?? new EntradaRisResponse
                {
                    CdRetrEci = 0,
                    CdRetrAces = 0,
                    Sucesso = true,
                    MensagemErro = null,
                    DataResposta = DateTime.Now
                });
            }

            public Task<EntradaRisRequest?> ObterRequisicaoPorIdAsync(int id)
            {
                return Task.FromResult<EntradaRisRequest?>(null);
            }
        }

        [TestMethod]
        public async Task ExecutarEntradaNfIcRisAsync_SuccessfulExecution_ReturnsSuccess()
        {
            // Arrange
            var repository = new TestEntradaRisRepository();
            var logger = LoggerFactory.Create(builder => { }).CreateLogger<EntradaRisService>();
            var service = new EntradaRisService(repository, logger);

            var input = new EnviarInterfaceRisInputDto
            {
                CdMercDst = "123",
                DtcSelFtrm = DateTime.Now,
                LgonFunc = "TestUser",
                AreParm = "TestParams"
            };

            // Act
            var result = await service.ExecutarEntradaNfIcRisAsync(input);

            // Assert
            Assert.IsTrue(result.Sucesso);
            Assert.AreEqual("Interface RIS processada com sucesso", result.Mensagem);
            Assert.IsNotNull(repository.LastRequest);
            Assert.AreEqual(input.CdMercDst, repository.LastRequest.CdMercDst);
        }

        [TestMethod]
        public async Task ExecutarEntradaNfIcRisAsync_ErrorInResponse_ReturnsError()
        {
            // Arrange
            var repository = new TestEntradaRisRepository
            {
                MockResponse = new EntradaRisResponse
                {
                    CdRetrEci = 1,
                    CdRetrAces = 2,
                    Sucesso = false,
                    MensagemErro = "Test Error",
                    DataResposta = DateTime.Now
                }
            };
            var logger = LoggerFactory.Create(builder => { }).CreateLogger<EntradaRisService>();
            var service = new EntradaRisService(repository, logger);

            var input = new EnviarInterfaceRisInputDto
            {
                CdMercDst = "123",
                DtcSelFtrm = DateTime.Now,
                LgonFunc = "TestUser",
                AreParm = "TestParams"
            };

            // Act
            var result = await service.ExecutarEntradaNfIcRisAsync(input);

            // Assert
            Assert.IsFalse(result.Sucesso);
            Assert.AreEqual("Erro na interface RIS", result.Mensagem);
            Assert.AreEqual("Test Error", result.MensagemErro);
        }
    }
}