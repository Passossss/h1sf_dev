using H1SF.Application.DTOs.EntradaNfIcRis;
using H1SF.Domain.Entities.EntradaNfIcRis;
using H1SF.Infrastructure.Repositories.EntradaNfIcRis;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.EntradaNfIcRis
{
    public class InterfaceRisService : IInterfaceRisService
    {
        private readonly IInterfaceRisRepository _repository;
        private readonly ILogger<InterfaceRisService> _logger;

        public InterfaceRisService(
            IInterfaceRisRepository repository,
            ILogger<InterfaceRisService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<EnviarInterfaceRisOutputDto> ExecutarEntradaNfIcRisAsync(EnviarInterfaceRisInputDto input)
        {
            _logger.LogInformation("Executando seção 573-00-ENTRADA-NF-IC-RIS");

            var resultado = new EnviarInterfaceRisOutputDto
            {
                DataExecucao = DateTime.Now
            };

            try
            {
                // 1. Preparar requisição RIS (equivalente aos MOVE do COBOL)
                var request = new InterfaceRisRequest
                {
                    CdAces = "02",                      // MOVE '02' TO WX04-CD-ACES
                    CdRetrEci = 0,                      // MOVE ZEROS TO WX04-CD-RETR-ECI
                    CdRetrAces = 0,                     // MOVE ZEROS TO WX04-CD-RETR-ACES
                    IcTAcao = "I",                      // MOVE 'I' TO WX04-IC-T-ACAO
                    CdMercDst = input.CdMercDst,        // MOVE WQ02-CD-MERC-DST TO WX04-CD-MERC-DST
                    DtcSelFtrm = input.DtcSelFtrm,      // MOVE WQ02-DTC-SEL-FTRM TO WX04-DTC-SEL-FTRM
                    LgonFunc = input.LgonFunc,          // MOVE WQ02-LGON-FUNC TO WX04-LGON-FUNC
                    IdPtcDsp = string.Empty,            // MOVE SPACES TO WX04-ID-PTC-DSP
                    IdImprFtrm = string.Empty,          // MOVE SPACES TO WX04-ID-IMPR-FTRM
                    LgonFuncRsp = input.LgonFunc,       // MOVE WQ02-LGON-FUNC TO WX04-LGON-FUNC-RSP
                    AreParm = input.AreParm             // Mantém área de parâmetros
                };

                // 2. Salvar requisição no banco (para auditoria)
                var idRequisicao = await _repository.SalvarRequisicaoAsync(request);
                resultado.IdRequisicao = idRequisicao;

                // 3. Executar link para interface RIS (PERFORM 584-00-EMITE-LINK-H1SF5008-02)
                var response = await _repository.EmitirLinkParaInterfaceRisAsync(request);

                // 4. Atualizar com resposta
                request.CdRetrEci = response.CdRetrEci;
                request.CdRetrAces = response.CdRetrAces;
                request.Sucesso = response.Sucesso;
                request.MensagemErro = response.MensagemErro;

                await _repository.AtualizarRespostaAsync(request.Id, response);

                // 5. Verificar retorno (IF WX04-CD-RETR-ECI NOT EQUAL ZEROS OR...)
                if (response.CdRetrEci != 0 || response.CdRetrAces != 0)
                {
                    // Equivalente ao tratamento de erro do COBOL
                    resultado.Sucesso = false;
                    resultado.ErroInterface = true;
                    resultado.CdRetrEci = response.CdRetrEci;
                    resultado.CdRetrAces = response.CdRetrAces;
                    resultado.CdAcesErro = "02";
                    resultado.CdErroEci = response.CdRetrEci.ToString();
                    resultado.CdErroAces = response.CdRetrAces.ToString();
                    resultado.MensagemErro = MontarMensagemErro(response, input.AreParm);
                    resultado.Mensagem = "Erro na interface RIS";

                    _logger.LogError(
                        "Erro interface RIS. ECI: {CdRetrEci}, ACES: {CdRetrAces}, Parâmetros: {AreParm}",
                        response.CdRetrEci, response.CdRetrAces, input.AreParm);
                }
                else
                {
                    resultado.Sucesso = true;
                    resultado.CdRetrEci = 0;
                    resultado.CdRetrAces = 0;
                    resultado.Mensagem = "Interface RIS processada com sucesso";

                    _logger.LogInformation("Interface RIS processada com sucesso. ID: {IdRequisicao}", idRequisicao);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na execução da entrada NF IC RIS");

                resultado.Sucesso = false;
                resultado.Mensagem = $"Erro interno: {ex.Message}";
                resultado.MensagemErro = ex.Message;
            }

            return resultado;
        }

        private string? MontarMensagemErro(Domain.Entities.EntradaNfIcRis.InterfaceRisResponse response, string areParm)
        {
            throw new NotImplementedException();
        }

        public async Task<EnviarInterfaceRisOutputDto> EnviarParaInterfaceRisAsync(EnviarInterfaceRisInputDto input)
        {
            // Método alternativo para uso direto
            return await ExecutarEntradaNfIcRisAsync(input);
        }

        private string MontarMensagemErro(InterfaceRisResponse response, string areParm)
        {
            // Equivalente à montagem da mensagem de erro do COBOL
            return $"*** ERRO - GRAVACAO INTERFACE RIS (MQ): ==> " +
                   $"Código Acesso: 02, " +
                   $"ECI: {response.CdRetrEci}, " +
                   $"ACES: {response.CdRetrAces}, " +
                   $"Parâmetros: {areParm}";
        }
    }

    // Classe para resposta da interface
    public class InterfaceRisResponse
    {
        public int CdRetrEci { get; set; }
        public int CdRetrAces { get; set; }
        public bool Sucesso { get; set; }
        public string? MensagemErro { get; set; }
        public DateTime DataResposta { get; set; }
    }
}
