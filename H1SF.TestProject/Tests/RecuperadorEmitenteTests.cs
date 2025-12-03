using H1SF.Application.Services.Emitente;
using H1SF.Domain.Entities.Emitente;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class RecuperadorEmitenteTests
    {
        private class TestRecuperadorEmitente : IRecuperadorEmitente
        {
            private readonly Dictionary<string, DadosEmitente> _emitentes;

            public TestRecuperadorEmitente(Dictionary<string, DadosEmitente> emitentes)
            {
                _emitentes = emitentes;
            }

            public Task<DadosEmitente> RecuperarEmitenteAsync(string idCnpj)
            {
                _emitentes.TryGetValue(idCnpj, out var emitente);
                return Task.FromResult(emitente);
            }
        }

        [TestMethod]
        public async Task RecuperarEmitenteAsync_ValidCnpj_ReturnsEmitente()
        {
            // Arrange
            var emitentes = new Dictionary<string, DadosEmitente>
            {
                { "12345678000195", new DadosEmitente { RazaoSocial = "Empresa Teste", CpfCgc = "12345678000195" } }
            };
            var service = new TestRecuperadorEmitente(emitentes);

            // Act
            var result = await service.RecuperarEmitenteAsync("12345678000195");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Empresa Teste", result.RazaoSocial);
            Assert.AreEqual("12345678000195", result.CpfCgc);
        }

        [TestMethod]
        public async Task RecuperarEmitenteAsync_InvalidCnpj_ReturnsNull()
        {
            // Arrange
            var emitentes = new Dictionary<string, DadosEmitente>();
            var service = new TestRecuperadorEmitente(emitentes);

            // Act
            var result = await service.RecuperarEmitenteAsync("00000000000000");

            // Assert
            Assert.IsNull(result);
        }
    }
}