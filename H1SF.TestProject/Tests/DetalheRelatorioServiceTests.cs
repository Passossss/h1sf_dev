using System.Threading.Tasks;
using H1SF.Application.DTOs.DreDetalhesRelatorio;
using H1SF.Application.Services.DreDetalhesRelatorio;
using H1SF.Domain.Entities.DreDetalhesRelatorio;
using H1SF.Infrastructure.Repositories.DreDetalhesRelatorio;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DetalheRelatorioServiceTests
    {
        private class TestDetalheRelatorioRepository : IDetalheRelatorioRepository
        {
            private readonly int _proximaSequencia;
            private readonly string _viaImpressao;
            private readonly bool _inserirResultado;

            public DetalheRelatorio? LastDetalhe { get; private set; }

            public TestDetalheRelatorioRepository(int proximaSequencia = 1, string viaImpressao = "  ", bool inserirResultado = true)
            {
                _proximaSequencia = proximaSequencia;
                _viaImpressao = viaImpressao;
                _inserirResultado = inserirResultado;
            }

            public Task<int> ObterProximaSequenciaAsync() => Task.FromResult(_proximaSequencia);

            public Task<string> DefinirViaImpressaoAsync() => Task.FromResult(_viaImpressao);

            public Task<bool> InserirDetalheAsync(DetalheRelatorio detalhe)
            {
                LastDetalhe = detalhe;
                return Task.FromResult(_inserirResultado);
            }
        }

        // Use a simple logger factory without AddConsole (avoids needing the Console logging package)
        private ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => { }).CreateLogger<T>();

        [TestMethod]
        public async Task ExecutarInsercaoDetalheAsync_LinhaVazia_DeveBypassar()
        {
            // Arrange
            var repo = new TestDetalheRelatorioRepository();
            var logger = CreateLogger<DetalheRelatorioService>();
            var service = new DetalheRelatorioService(repo, logger);

            var input = new InserirDetalheInputDto
            {
                DreCnLnhRel = " ", // linha vazia -> bypass
                CdRegrFtrm = "",
                CdTPrd = "",
                DreIdPrcpPtdLit = "",
                DreCdSqnPjl = "",
                CdSqnPjlNf = ""
            };

            // Act
            var resultado = await service.ExecutarInsercaoDetalheAsync(input);

            // Assert
            Assert.IsTrue(resultado.Sucesso);
            Assert.IsFalse(resultado.ExecutouInsert);
            Assert.AreEqual("N", resultado.ChaveComandoPjl);
            StringAssert.Contains(resultado.Mensagem, "bypass", "Mensagem deve indicar bypass");
        }

        [TestMethod]
        public async Task ExecutarInsercaoDetalheAsync_ProtocolCondition_DeveBypassar()
        {
            // Arrange
            var repo = new TestDetalheRelatorioRepository();
            var logger = CreateLogger<DetalheRelatorioService>();
            var service = new DetalheRelatorioService(repo, logger);

            var input = new InserirDetalheInputDto
            {
                DreCnLnhRel = "X", // not empty
                CdRegrFtrm = "M",
                CdTPrd = "",
                DreIdPrcpPtdLit = "PROTOCOL",
                DreCdSqnPjl = "",
                CdSqnPjlNf = ""
            };

            // Act
            var resultado = await service.ExecutarInsercaoDetalheAsync(input);

            // Assert
            Assert.IsTrue(resultado.Sucesso);
            Assert.IsFalse(resultado.ExecutouInsert);
        }

        [TestMethod]
        public async Task ExecutarInsercaoDetalheAsync_RegraN_ComProjetoNf_DeveBypassar()
        {
            // Arrange
            var repo = new TestDetalheRelatorioRepository();
            var logger = CreateLogger<DetalheRelatorioService>();
            var service = new DetalheRelatorioService(repo, logger);

            var input = new InserirDetalheInputDto
            {
                DreCnLnhRel = "X",
                CdRegrFtrm = "N",
                CdTPrd = "",
                DreIdPrcpPtdLit = "",
                DreCdSqnPjl = "ABC",
                CdSqnPjlNf = "ABC" // equals -> bypass
            };

            // Act
            var resultado = await service.ExecutarInsercaoDetalheAsync(input);

            // Assert
            Assert.IsTrue(resultado.Sucesso);
            Assert.IsFalse(resultado.ExecutouInsert);
        }

        [TestMethod]
        public async Task ExecutarInsercaoDetalheAsync_InsertSuccess_RetornaSequencia()
        {
            // Arrange
            var repo = new TestDetalheRelatorioRepository(proximaSequencia: 5, viaImpressao: " A ", inserirResultado: true);
            var logger = CreateLogger<DetalheRelatorioService>();
            var service = new DetalheRelatorioService(repo, logger);

            var input = new InserirDetalheInputDto
            {
                DreCnLnhRel = "X",
                CdRegrFtrm = "Z",
                CdTPrd = "Z",
                DreIdPrcpPtdLit = "NOT_PROTOCOL",
                DreCdSqnPjl = "01",
                CdSqnPjlNf = "99",
                DreCdStm = "STM",
                DreDtcGrc = "20231203153045", // valid 14-char COBOL datetime
                DreIdPrcp = "PRCP",
                DreCdSqnDct = "DCT"
            };

            // Act
            var resultado = await service.ExecutarInsercaoDetalheAsync(input);

            // Assert
            Assert.IsTrue(resultado.Sucesso);
            Assert.IsTrue(resultado.ExecutouInsert);
            Assert.AreEqual(5, resultado.SequenciaLinha);
            StringAssert.Contains(resultado.Mensagem, "Sequência: 5");
            Assert.IsNotNull(repo.LastDetalhe);
            Assert.AreEqual(5, repo.LastDetalhe.DreCdSqnLnh);
            // Via impressão spaces replaced by zeros: " A " -> "0A0"
            Assert.AreEqual("0A0", repo.LastDetalhe.DreIdVia);
        }

        [TestMethod]
        public async Task ExecutarInsercaoDetalheAsync_InsertFails_RetornaErro()
        {
            // Arrange
            var repo = new TestDetalheRelatorioRepository(proximaSequencia: 2, viaImpressao: "  ", inserirResultado: false);
            var logger = CreateLogger<DetalheRelatorioService>();
            var service = new DetalheRelatorioService(repo, logger);

            var input = new InserirDetalheInputDto
            {
                DreCnLnhRel = "X",
                CdRegrFtrm = "Z",
                CdTPrd = "Z",
                DreIdPrcpPtdLit = "NOPROTO",
                DreCdSqnPjl = "01",
                CdSqnPjlNf = "99",
                DreCdStm = "STM",
                DreDtcGrc = "20231203153045",
                DreIdPrcp = "PRCP",
                DreCdSqnDct = "DCT"
            };

            // Act
            var resultado = await service.ExecutarInsercaoDetalheAsync(input);

            // Assert
            Assert.IsFalse(resultado.Sucesso);
            Assert.IsFalse(resultado.ExecutouInsert);
            StringAssert.Contains(resultado.Mensagem, "Erro ao inserir detalhe");
        }
    }
}