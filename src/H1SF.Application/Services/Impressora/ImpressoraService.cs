using H1SF.Application.DTO;
using H1SF.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class ImpressoraService : IImpressoraService
    {
        private readonly IDefinirImpressoraRepository _repository;
        private readonly ILogger<ImpressoraService> _logger;

        public ImpressoraService(IDefinirImpressoraRepository repository, ILogger<ImpressoraService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DefinirImpressoraOutputDto> DefinirImpressoraAsync(DefinirImpressoraInputDto input)
        {
            try
            {
                _logger.LogInformation("Iniciando definição de impressora para CD_MERC_DST: {CdMercDst}", input.CdMercDst);

                // Passo 1: Buscar seleção de faturamento
                var selecaoFaturamento = await _repository.ObterSelecaoFaturamentoAsync(
                    input.CdMercDst,
                    input.DtcSelFtrm,
                    input.LgonFunc);

                if (selecaoFaturamento == null || selecaoFaturamento.SftIdImprFtrm <= 0)
                {
                    // Corresponde à validação do COBOL: SQLCODE NOT FOUND ou ID < 0
                    _logger.LogError("Erro na definição de impressora. DTC_SEL_FTRM: {Data}", input.DtcSelFtrm);

                    return new DefinirImpressoraOutputDto
                    {
                        Sucesso = false,
                        MensagemErro = $"*** ERRO - DEFINICAO DE IMPRESSORA  ===> {input.DtcSelFtrm}",
                        CodigoErro = "H1SF0033"
                    };
                }

                // Passo 2: Buscar nome da impressora
                var nomeImpressora = await _repository.ObterNomeImpressoraAsync(selecaoFaturamento.SftIdImprFtrm);

                if (string.IsNullOrEmpty(nomeImpressora))
                {
                    // Corresponde à validação: impressora inexistente
                    _logger.LogError("Impressora inexistente. ID_IMPR: {IdImpressora}", selecaoFaturamento.SftIdImprFtrm);

                    return new DefinirImpressoraOutputDto
                    {
                        Sucesso = false,
                        MensagemErro = $"*** ERRO - IMPRESSORA INEXISTENTE  ===> {selecaoFaturamento.SftIdImprFtrm}",
                        CodigoErro = "H1SF0033"
                    };
                }

                // Passo 3: Retornar resultado (equivalente ao MOVE para WS01-IMPRESSORA-LASER)
                _logger.LogInformation("Impressora definida com sucesso: {NomeImpressora}", nomeImpressora);

                return new DefinirImpressoraOutputDto
                {
                    NomeImpressora = nomeImpressora,
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao definir impressora");

                return new DefinirImpressoraOutputDto
                {
                    Sucesso = false,
                    MensagemErro = "Erro interno ao processar definição de impressora",
                    CodigoErro = "INTERNAL_ERROR"
                };
            }
        }

        // Método adicional para obter tipo de recolhimento (se necessário)
        public async Task<int?> ObterTipoRecolhimentoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
        {
            return await _repository.ObterIdTipoRecolhimentoAsync(cdMercDst, dtcSelFtrm, lgonFunc);
        }
    }
}
