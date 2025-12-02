using H1SF.Domain.Entities.Emitente;
using H1SF.Infrastructure.Repositories.Emitente;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Emitente;

/// <summary>
/// 505-00-RECUPERA-EMITENTE - Recupera dados do emitente
/// Linhas COBOL: 3294-3383
/// </summary>
public class RecuperadorEmitente : IRecuperadorEmitente
{
    private readonly IEmitenteRepository _repository;
    private readonly ILogger<RecuperadorEmitente> _logger;

    public RecuperadorEmitente(
        IEmitenteRepository repository,
        ILogger<RecuperadorEmitente> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 505-00-RECUPERA-EMITENTE SECTION
    /// MOVE CB0004-ID-CNPJ TO WQ01-EMITENTE-PFJ-CODIGO
    /// MOVE '1' TO WQ01-EMITENTE-LOC-CODIGO
    /// </summary>
    public async Task<DadosEmitente> RecuperarEmitenteAsync(string idCnpj)
    {
        try
        {
            // COBOL: MOVE CB0004-ID-CNPJ TO WQ01-EMITENTE-PFJ-CODIGO
            // COBOL: MOVE '1' TO WQ01-EMITENTE-LOC-CODIGO
            const string locCodigo = "1";

            _logger.LogDebug("Recuperando emitente: CNPJ={IdCnpj}", idCnpj);

            var resultado = await _repository.ObterDadosEmitenteAsync(idCnpj, locCodigo);

            // COBOL: IF SQLCODE EQUAL WS31-CHV-NTFD-SQL
            //            MOVE '*** ERRO - EMITENTE NAO ENCONTRADO  ===>' TO WS35-MSG-ERRO-LIT
            if (resultado == null)
            {
                _logger.LogError("Emitente não encontrado: CNPJ={IdCnpj}", idCnpj);
                throw new InvalidOperationException(
                    $"*** ERRO - EMITENTE NAO ENCONTRADO ===> CNPJ: {idCnpj}");
            }

            _logger.LogInformation(
                "Emitente recuperado com sucesso: RazaoSocial={RazaoSocial}, CNPJ={CpfCgc}",
                resultado.RazaoSocial, resultado.CpfCgc);

            return resultado;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Erro ao recuperar emitente: CNPJ={IdCnpj}", idCnpj);
            throw;
        }
    }
}
