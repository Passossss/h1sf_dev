using H1SF.Domain.Entities.Emitente;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace H1SF.Infrastructure.Repositories.Emitente;

/// <summary>
/// 505-00-RECUPERA-EMITENTE - Recupera dados do emitente
/// Linhas COBOL: 3294-3383
/// </summary>
public class EmitenteRepository : IEmitenteRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmitenteRepository> _logger;

    public EmitenteRepository(
        ApplicationDbContext context,
        ILogger<EmitenteRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 505-00-RECUPERA-EMITENTE SECTION
    /// EXEC SQL AT CCPR
    ///          SELECT A.MNEMONICO, INITCAP(A.MNEMONICO), ...
    ///          INTO   :CC0008-RAZAO-SOCIAL-E, :CC0008-RAZAO-SOCIAL-EPM, ...
    ///          FROM   B8CC.COR_PESSOA A, B8CC.COR_LOCALIDADE_VIGENCIA B,
    ///                 B8CC.COR_MUNICIPIO C, B8CC.COR_PESSOA_VIGENCIA D
    ///          WHERE  A.PFJ_CODIGO = B.PFJ_CODIGO
    ///          AND    B.MUN_CODIGO = C.MUN_CODIGO
    ///          AND    A.PFJ_CODIGO = D.PFJ_CODIGO
    ///          AND    B.DT_INICIO <= SYSDATE AND (B.DT_FIM >= SYSDATE OR B.DT_FIM IS NULL)
    ///          AND    D.DT_INICIO <= SYSDATE AND (D.DT_FIM >= SYSDATE OR D.DT_FIM IS NULL)
    ///          AND    A.PFJ_CODIGO = :WQ01-EMITENTE-PFJ-CODIGO
    ///          AND    B.LOC_CODIGO = :WQ01-EMITENTE-LOC-CODIGO
    /// </summary>
    public async Task<DadosEmitente?> ObterDadosEmitenteAsync(string idCnpj, string locCodigo)
    {
        try
        {
            var dataAtual = DateTime.Now;

            // COBOL: Query complexa com múltiplos JOINs e condições de vigência
            var query = await (
                from pessoa in _context.CorPessoas
                join localidade in _context.CorLocalidadeVigencias
                    on pessoa.PfjCodigo equals localidade.PfjCodigo
                join municipio in _context.CorMunicipios
                    on localidade.MunCodigo equals municipio.MunCodigo
                join pessoaVigencia in _context.CorPessoaVigencias
                    on pessoa.PfjCodigo equals pessoaVigencia.PfjCodigo
                where pessoa.PfjCodigo == idCnpj &&
                      localidade.LocCodigo == locCodigo &&
                      localidade.DtInicio <= dataAtual &&
                      (localidade.DtFim >= dataAtual || localidade.DtFim == null) &&
                      pessoaVigencia.DtInicio <= dataAtual &&
                      (pessoaVigencia.DtFim >= dataAtual || pessoaVigencia.DtFim == null)
                select new
                {
                    Mnemonico = pessoa.Mnemonico,
                    Logradouro = localidade.Logradouro ?? string.Empty,
                    Numero = localidade.Numero ?? string.Empty,
                    Bairro = localidade.Bairro ?? string.Empty,
                    Cep = localidade.Cep ?? string.Empty,
                    Municipio = localidade.Municipio ?? string.Empty,
                    UfCodigo = municipio.UfCodigo,
                    CpfCgc = pessoaVigencia.CpfCgc ?? string.Empty,
                    InscrEstadual = localidade.InscrEstadual ?? string.Empty,
                    Complemento = localidade.Complemento ?? string.Empty
                })
                .FirstOrDefaultAsync();

            if (query == null)
            {
                _logger.LogWarning(
                    "Emitente não encontrado: IdCnpj={IdCnpj}, LocCodigo={LocCodigo}",
                    idCnpj, locCodigo);
                return null;
            }

            // COBOL: Montagem dos campos concatenados conforme SQL original
            var enderecoCompleto = query.Logradouro +
                (string.IsNullOrEmpty(query.Numero) ? "" : ", " + query.Numero);

            var bairroMunUf = (!string.IsNullOrEmpty(query.Bairro) ? query.Bairro + ", " : "") +
                query.Municipio + ", " + query.UfCodigo;

            var bairroCep = query.Bairro +
                (string.IsNullOrEmpty(query.Bairro) ? "" : ", ") + query.Cep;

            var munUf = query.Municipio + ", " + query.UfCodigo;

            // COBOL: INITCAP(A.MNEMONICO) - primeira letra maiúscula de cada palavra
            var razaoSocialPm = ToTitleCase(query.Mnemonico);

            var resultado = new DadosEmitente
            {
                // A.MNEMONICO
                RazaoSocial = query.Mnemonico,

                // INITCAP(A.MNEMONICO)
                RazaoSocialPm = razaoSocialPm,

                // B.LOGRADOURO || DECODE(B.NUMERO, '', '', ', ' || B.NUMERO)
                EnderecoEd = enderecoCompleto,
                EnderecoEp = enderecoCompleto,
                EnderecoEi = enderecoCompleto,

                // B.BAIRRO
                BairroEp = query.Bairro,

                // B.BAIRRO || ', ' || B.CEP
                BairroCepEd = bairroCep,

                // DECODE(B.BAIRRO, '', '', B.BAIRRO || ', ') || B.MUNICIPIO || ', ' || C.UF_CODIGO
                BairroMunNomeUfEp = bairroMunUf,
                BairroMunNomeUfEi = bairroMunUf,

                // B.CEP
                CepE = query.Cep,

                // B.MUNICIPIO || ', ' || C.UF_CODIGO
                MunNomeUfE = munUf,

                // C.UF_CODIGO
                UfE = query.UfCodigo,

                // 'CGC: ' || D.CPF_CGC
                CpfCgcE = "CGC: " + query.CpfCgc,

                // 'Insc. Estadual: ' || B.INSCR_ESTADUAL
                InscrEstadualE = "Insc. Estadual: " + query.InscrEstadual,

                // D.CPF_CGC
                CpfCgc = query.CpfCgc,

                // B.INSCR_ESTADUAL
                InscrEstadual = query.InscrEstadual,

                // 'CGC: ' || D.CPF_CGC || ' / ' || 'Insc. Estadual: ' || B.INSCR_ESTADUAL
                CpfCgcInscrEstEi = "CGC: " + query.CpfCgc + " / Insc. Estadual: " + query.InscrEstadual,

                // B.COMPLEMENTO
                Complemento = query.Complemento
            };

            _logger.LogDebug(
                "Dados do emitente recuperados: RazaoSocial={RazaoSocial}, CNPJ={CpfCgc}",
                resultado.RazaoSocial, resultado.CpfCgc);

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao recuperar dados do emitente: IdCnpj={IdCnpj}, LocCodigo={LocCodigo}",
                idCnpj, locCodigo);
            throw;
        }
    }

    /// <summary>
    /// Implementa INITCAP do Oracle - primeira letra maiúscula de cada palavra
    /// </summary>
    private static string ToTitleCase(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var textInfo = new CultureInfo("pt-BR", false).TextInfo;
        return textInfo.ToTitleCase(text.ToLower());
    }
}
