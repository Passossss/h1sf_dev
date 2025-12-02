using H1SF.Domain.Entities.Fabrica;
using H1SF.Infrastructure.Repositories.Fabrica;
using Microsoft.Extensions.Logging;

namespace H1SF.Application.Services.Fabrica;

/// <summary>
/// 572-00-SQL-RECUPERA-CNPJ - Recupera CNPJ da fábrica
/// Linhas COBOL: 4469-4555
/// </summary>
public class RecuperadorCnpjFabrica : IRecuperadorCnpjFabrica
{
    private readonly ICnpjFabricaRepository _repository;
    private readonly ILogger<RecuperadorCnpjFabrica> _logger;

    public RecuperadorCnpjFabrica(
        ICnpjFabricaRepository repository,
        ILogger<RecuperadorCnpjFabrica> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 572-00-SQL-RECUPERA-CNPJ SECTION
    /// IF WS36-CD-MERC-DST EQUAL 'D' AND WS36-FASE-FTRM EQUAL '2'
    ///     GO TO 572-20-SQL-CNPJ-TRIANG
    /// ELSE
    ///     572-10-SQL-CNPJ-VENDA
    /// </summary>
    public async Task<CnpjFabrica?> RecuperarCnpjAsync(
        string cdMercDst,
        string faseFtrm,
        int cdMercDstInt,
        DateTime dtcSelFtrm,
        string lgonFunc,
        string icSim)
    {
        try
        {
            CnpjFabrica? resultado;

            // COBOL: IF WS36-CD-MERC-DST EQUAL 'D' AND WS36-FASE-FTRM EQUAL '2'
            //            GO TO 572-20-SQL-CNPJ-TRIANG
            if (cdMercDst == "D" && faseFtrm == "2")
            {
                _logger.LogDebug("Recuperando CNPJ para triangulação");
                resultado = await _repository.ObterCnpjTriangulacaoAsync(
                    cdMercDstInt, dtcSelFtrm, lgonFunc, icSim);
            }
            else
            {
                _logger.LogDebug("Recuperando CNPJ para venda normal");
                resultado = await _repository.ObterCnpjVendaAsync(
                    cdMercDstInt, dtcSelFtrm, lgonFunc);
            }

            // COBOL: 572-30-TRATA-CNPJ
            // IF SQLCODE EQUAL WS31-CHV-NTFD-SQL
            //     MOVE '*** ERRO - CNPJ FABRICA NAO ENCONTRADO  ===>' TO WS35-MSG-ERRO-LIT
            if (resultado == null)
            {
                _logger.LogError(
                    "CNPJ fábrica não encontrado: CdMercDst={CdMercDst}, FaseFtrm={FaseFtrm}, DtcSelFtrm={DtcSelFtrm}",
                    cdMercDst, faseFtrm, dtcSelFtrm);
                
                throw new InvalidOperationException(
                    $"*** ERRO - CNPJ FABRICA NAO ENCONTRADO ===> DtcSelFtrm: {dtcSelFtrm:yyyyMMdd}");
            }

            // COBOL: INSPECT CB0004-ID-CNPJ CONVERTING SPACES TO ZEROS (já feito no repository)
            _logger.LogInformation(
                "CNPJ recuperado com sucesso: CdTPrd={CdTPrd}, IdCnpj={IdCnpj}, CdFbr={CdFbr}",
                resultado.CdTPrd, resultado.IdCnpj, resultado.CdFbr);

            return resultado;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Erro ao recuperar CNPJ da fábrica");
            throw;
        }
    }
}
