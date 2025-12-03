using H1SF.Application.DTOs.DreDetalhesRelatorio;
using H1SF.Domain.Entities.DreDetalhesRelatorio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.DreDetalhesRelatorio
{
    public class DetalheRelatorioService : IDetalheRelatorioService
    {
        private readonly IDetalheRelatorioRepository _repository;
        private readonly ILogger<DetalheRelatorioService> _logger;

        public DetalheRelatorioService(
            IDetalheRelatorioRepository repository,
            ILogger<DetalheRelatorioService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<InserirDetalheOutputDto> ExecutarInsercaoDetalheAsync(InserirDetalheInputDto input)
        {
            _logger.LogInformation("Executando seção 546-00-INSERT-DETALHE");

            var resultado = new InserirDetalheOutputDto
            {
                DataExecucao = DateTime.Now,
                ChaveComandoPjl = "N" // Default como no COBOL
            };

            try
            {
                // 1. Verificar condições de bypass
                bool deveBypass = DeveExecutarBypass(
                    input.DreCnLnhRel,
                    input.CdRegrFtrm,
                    input.CdTPrd,
                    input.DreIdPrcpPtdLit,
                    input.DreCdSqnPjl,
                    input.CdSqnPjlNf);

                if (deveBypass)
                {
                    _logger.LogInformation("Condição de bypass atendida. Resetando chave para 'N'.");
                    resultado.Sucesso = true;
                    resultado.ExecutouInsert = false;
                    resultado.Mensagem = "Execução bypass - chave resetada para 'N'";
                    return resultado;
                }

                // 2. Obter próxima sequência (ADD 1 TO WS01-CD-SQN-LNH)
                int proximaSequencia = await _repository.ObterProximaSequenciaAsync();

                // 3. Definir via de impressão (PERFORM 180-00-DEFINE-VIA-IMPRESS)
                string viaImpressao = await _repository.DefinirViaImpressaoAsync();

                // 4. Converter espaços para zeros (INSPECT CONVERTING SPACES TO ZEROS)
                viaImpressao = viaImpressao.Replace(' ', '0');

                // 5. Converter data do formato COBOL
                DateTime dataGrc = ConverterDataCobol(input.DreDtcGrc);

                // 6. Criar entidade
                var detalhe = new DetalheRelatorio
                {
                    DreCdStm = input.DreCdStm,
                    DreDtcGrc = dataGrc,
                    DreIdPrcp = input.DreIdPrcp,
                    DreCdSqnDct = input.DreCdSqnDct,
                    DreCdSqnPjl = input.DreCdSqnPjl,
                    DreCdSqnLnh = proximaSequencia,
                    DreCnLnhRel = input.DreCnLnhRel,
                    DreIdVia = viaImpressao
                };

                // 7. Executar INSERT
                bool inserido = await _repository.InserirDetalheAsync(detalhe);

                if (inserido)
                {
                    resultado.Sucesso = true;
                    resultado.ExecutouInsert = true;
                    resultado.SequenciaLinha = proximaSequencia;
                    resultado.Mensagem = $"Detalhe inserido. Sequência: {proximaSequencia}";
                }
                else
                {
                    resultado.Sucesso = false;
                    resultado.ExecutouInsert = false;
                    resultado.Mensagem = "Erro ao inserir detalhe";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na inserção de detalhe");
                resultado.Sucesso = false;
                resultado.Mensagem = $"Erro: {ex.Message}";
            }

            return resultado;
        }

        private bool DeveExecutarBypass(
            string dreCnLnhRel,
            string cdRegrFtrm,
            string cdTPrd,
            string dreIdPrcpPtdLit,
            string dreCdSqnPjl,
            string cdSqnPjlNf)
        {
            // Condição 1: Linha vazia
            if (string.IsNullOrWhiteSpace(dreCnLnhRel))
                return true;

            // Condição 2: (Regra 'M' ou 'K' ou produto 'C') E protocolo
            if ((cdRegrFtrm == "M" || cdRegrFtrm == "K" || cdTPrd == "C")
                && dreIdPrcpPtdLit == "PROTOCOL")
                return true;

            // Condição 3: Regra 'N' E (projeto = NF ou '97' ou '99')
            if (cdRegrFtrm == "N"
                && (dreCdSqnPjl == cdSqnPjlNf
                    || dreCdSqnPjl == "97"
                    || dreCdSqnPjl == "99"))
                return true;

            return false;
        }

        private DateTime ConverterDataCobol(string dataCobol)
        {
            if (string.IsNullOrWhiteSpace(dataCobol) || dataCobol.Length != 14)
                return DateTime.Now;

            try
            {
                return DateTime.ParseExact(
                    dataCobol,
                    "yyyyMMddHHmmss",
                    System.Globalization.CultureInfo.InvariantCulture
                );
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
