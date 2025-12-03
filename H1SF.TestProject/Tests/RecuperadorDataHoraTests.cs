using H1SF.Infrastructure.Repositories.DataHora;

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
    }
}