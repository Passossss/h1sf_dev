using H1SF.Infrastructure.Repositories.FaturamentoPws;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.FaturamentoPws
{
    public class AtualizarPwsService : IAtualizarPwsService
    {
        private readonly IAtualizarPwsRepository _repository;
        private readonly ILogger<AtualizarPwsService> _logger;

        public AtualizarPwsService(IAtualizarPwsRepository repository, ILogger<AtualizarPwsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AtualizarPwsOutputDto> ExecutarAtualizacaoPwsAsync(AtualizarPwsInputDto input)
        {
            _logger.LogInformation("Iniciando atualização PWS. CD_MERC_DST: {CdMercDst}, FASE: {FaseFtrm}", input.CdMercDst, input.FaseFtrm);

            var resultado = new AtualizarPwsOutputDto { DataExecucao = DateTime.Now };

            try
            {
                if (!await _repository.DeveExecutarAtualizacaoAsync(input.SftIcNaczIcpnBt, input.SftCdTRec))
                {
                    resultado.Sucesso = true;
                    resultado.Mensagem = "Atualização não necessária (condição de bypass)";
                    return resultado;
                }

                // Processar Itens
                var itensAgrupados = await _repository.ObterItensAgrupadosAsync(input.CdMercDst, input.DtcSelFtrm, input.LgonFunc, input.FaseFtrm);

                foreach (var item in itensAgrupados)
                {
                    var atualizado = await _repository.AtualizarItemRecolhimentoAsync(item.ItdIdEtiqRec, item.SomaQuantidade);

                    if (!atualizado)
                    {
                        var soma = await _repository.ObterSomaQuantidadesItemAsync(input.CdMercDst, input.DtcSelFtrm, input.LgonFunc, input.FaseFtrm, item.ItdIdEtiqRec);
                        if (soma.HasValue)
                        {
                            atualizado = await _repository.AtualizarItemRecolhimentoAsync(item.ItdIdEtiqRec, soma.Value);
                        }
                    }

                    if (atualizado) resultado.ItensAtualizados++;
                }

                // Processar Volumes
                var volumes = await _repository.ObterVolumesDistintosAsync(input.CdMercDst, input.DtcSelFtrm, input.LgonFunc, input.FaseFtrm);

                foreach (var volume in volumes)
                {
                    if (input.CdMercDstW36 == "D" && !string.IsNullOrEmpty(volume.ItdIdNf))
                    {
                        await _repository.AtualizarIdDocumentoFiscalAsync(volume.ItdIdVol, volume.ItdIdNf);
                    }

                    bool deveAtualizarFaturado = DeveAtualizarVolumeFaturado(input.CdMercDstW36, input.FaseFtrmW36, input.SftIcFtrmTrgd, input.CdRegrFtrm);

                    if (deveAtualizarFaturado)
                    {
                        var atualizado = await _repository.AtualizarVolumeFaturadoAsync(volume.ItdIdVol);
                        if (atualizado) resultado.VolumesAtualizados++;
                    }
                }

                resultado.Sucesso = true;
                resultado.Mensagem = $"Atualização PWS concluída. Itens: {resultado.ItensAtualizados}, Volumes: {resultado.VolumesAtualizados}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a atualização PWS");
                resultado.Sucesso = false;
                resultado.Mensagem = $"Erro: {ex.Message}";
            }

            return resultado;
        }

        private bool DeveAtualizarVolumeFaturado(string? cdMercDstW36, string? faseFtrmW36, string? sftIcFtrmTrgd, string? cdRegrFtrm)
        {
            if (cdMercDstW36 == "D")
            {
                if ((faseFtrmW36 == "1" && sftIcFtrmTrgd != "S") || faseFtrmW36 == "2")
                    return true;
            }
            else if (cdMercDstW36 == "E")
            {
                if ((faseFtrmW36 == "1" && (cdRegrFtrm == "J" || cdRegrFtrm == "N")) || faseFtrmW36 == "2")
                    return true;
            }
            return false;
        }
    }
}
