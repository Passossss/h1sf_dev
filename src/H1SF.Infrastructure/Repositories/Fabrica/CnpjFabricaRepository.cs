using H1SF.Domain.Entities.Fabrica;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace H1SF.Infrastructure.Repositories.Fabrica;

/// <summary>
/// 572-00-SQL-RECUPERA-CNPJ - Recupera CNPJ da fábrica
/// Linhas COBOL: 4469-4555
/// </summary>
public class CnpjFabricaRepository : ICnpjFabricaRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CnpjFabricaRepository> _logger;

    public CnpjFabricaRepository(
        ApplicationDbContext context,
        ILogger<CnpjFabricaRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 572-10-SQL-CNPJ-VENDA
    /// EXEC SQL SELECT A.CD_T_PRD, A.ID_CNPJ, A.CD_FBR
    ///          INTO   :CB0004-CD-T-PRD, :CB0004-ID-CNPJ, :CB0004-CD-FBR
    ///          FROM   H1CB.FABRICA A, H1ST.TIPO_RECOLHIMENTO B
    ///          WHERE  A.CD_FBR = B.CD_FBR
    ///          AND    A.CD_T_PRD = B.CD_T_PRD
    ///          AND    A.NM_CDAD = B.NM_CDAD
    ///          AND    B.CD_T_REC IN (SELECT C.SFT_CD_T_REC FROM H1SF.SFT_SELECAO_FTRM C
    ///                                WHERE C.SFT_CD_MERC_DST = :WQ02-CD-MERC-DST
    ///                                AND C.SFT_DTC_SEL_FTRM = :WQ02-DTC-SEL-FTRM
    ///                                AND C.SFT_LGON_FUNC = :WQ02-LGON-FUNC
    ///                                AND ROWNUM = 1)
    /// </summary>
    public async Task<CnpjFabrica?> ObterCnpjVendaAsync(
        int cdMercDst, 
        DateTime dtcSelFtrm, 
        string lgonFunc)
    {
        try
        {
            // COBOL: Subquery para obter CD_T_REC
            var cdTRec = await _context.SelecoesFaturamento
                .Where(c => c.SftCdMercDst == cdMercDst &&
                           c.SftDtcSelFtrm == dtcSelFtrm &&
                           c.SftLgonFunc == lgonFunc)
                .Select(c => (int?)c.SftCdTRec)
                .FirstOrDefaultAsync();

            if (!cdTRec.HasValue)
            {
                _logger.LogWarning(
                    "Nenhum tipo de recolhimento encontrado para CdMercDst={CdMercDst}, DtcSelFtrm={DtcSelFtrm}, LgonFunc={LgonFunc}",
                    cdMercDst, dtcSelFtrm, lgonFunc);
                return null;
            }

            // COBOL: Query principal com JOIN entre FABRICA e TIPO_RECOLHIMENTO
            var resultado = await (
                from fabrica in _context.Fabricas
                join tipoRec in _context.TiposRecolhimento 
                    on new { fabrica.CdFbr, fabrica.CdTPrd } 
                    equals new { tipoRec.CdFbr, CdTPrd = tipoRec.CdTPrd ?? string.Empty }
                where tipoRec.CdTRec == cdTRec.Value
                select new CnpjFabrica
                {
                    CdTPrd = fabrica.CdTPrd,
                    IdCnpj = fabrica.IdCnpj ?? string.Empty,
                    CdFbr = fabrica.CdFbr
                })
                .FirstOrDefaultAsync();

            if (resultado != null)
            {
                // COBOL: INSPECT CB0004-ID-CNPJ CONVERTING SPACES TO ZEROS
                resultado.IdCnpj = resultado.IdCnpj.Replace(" ", "0");
                
                _logger.LogDebug(
                    "CNPJ Venda recuperado: CdTPrd={CdTPrd}, IdCnpj={IdCnpj}, CdFbr={CdFbr}",
                    resultado.CdTPrd, resultado.IdCnpj, resultado.CdFbr);
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Erro ao recuperar CNPJ venda: CdMercDst={CdMercDst}, DtcSelFtrm={DtcSelFtrm}, LgonFunc={LgonFunc}",
                cdMercDst, dtcSelFtrm, lgonFunc);
            throw;
        }
    }

    /// <summary>
    /// 572-20-SQL-CNPJ-TRIANG
    /// EXEC SQL SELECT A.CD_T_PRD, A.ID_CNPJ, A.CD_FBR
    ///          INTO   :CB0004-CD-T-PRD, :CB0004-ID-CNPJ, :CB0004-CD-FBR
    ///          FROM   H1CB.FABRICA A, H1ST.CONTROLE_VOLUME B, H1ST.TIPO_RECOLHIMENTO C, H1ST.CONTROLE_VOLUME E
    ///          WHERE  A.CD_FBR = C.CD_FBR
    ///          AND    A.CD_T_PRD = C.CD_T_PRD
    ///          AND    A.CD_UF = B.CD_UF_DST
    ///          AND    A.IC_FBR_TRGD = :WQ01-IC-SIM
    ///          AND    E.ID_CLI = A.ID_CLI
    ///          AND    EXISTS (SELECT 1 FROM H1SF.SFT_SELECAO_FTRM D
    ///                         WHERE D.SFT_CD_T_REC = C.CD_T_REC
    ///                         AND D.SFT_CD_T_MTZ = B.CD_T_MTZ
    ///                         AND D.SFT_ID_MTZ = B.ID_MTZ
    ///                         AND D.SFT_ID_CLI = B.ID_CLI
    ///                         AND D.SFT_CD_MERC_DST = :WQ02-CD-MERC-DST
    ///                         AND D.SFT_DTC_SEL_FTRM = :WQ02-DTC-SEL-FTRM
    ///                         AND D.SFT_LGON_FUNC = :WQ02-LGON-FUNC
    ///                         AND ROWNUM = 1)
    /// </summary>
    public async Task<CnpjFabrica?> ObterCnpjTriangulacaoAsync(
        int cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc,
        string icSim)
    {
        try
        {
            // COBOL: Query complexa com múltiplos JOINs e EXISTS
            var resultado = await (
                from fabrica in _context.Fabricas
                from controleVolumeB in _context.ControlesVolume
                    .Where(b => b.CdUfDst == fabrica.CdUf)
                from tipoRec in _context.TiposRecolhimento
                    .Where(c => c.CdFbr == fabrica.CdFbr &&
                               c.CdTPrd == fabrica.CdTPrd)
                from controleVolumeE in _context.ControlesVolume
                    .Where(e => e.IdCli == fabrica.IdCli)
                where fabrica.IcFbrTrgd == icSim &&
                      _context.SelecoesFaturamento.Any(d =>
                          d.SftCdTRec == tipoRec.CdTRec &&
                          d.SftCdTMtz == controleVolumeB.CdTMtz &&
                          d.SftIdMtz == controleVolumeB.IdMtz &&
                          d.SftIdCli == controleVolumeB.IdCli &&
                          d.SftCdMercDst == cdMercDst &&
                          d.SftDtcSelFtrm == dtcSelFtrm &&
                          d.SftLgonFunc == lgonFunc)
                select new CnpjFabrica
                {
                    CdTPrd = fabrica.CdTPrd,
                    IdCnpj = fabrica.IdCnpj ?? string.Empty,
                    CdFbr = fabrica.CdFbr
                })
                .FirstOrDefaultAsync();

            if (resultado != null)
            {
                // COBOL: INSPECT CB0004-ID-CNPJ CONVERTING SPACES TO ZEROS
                resultado.IdCnpj = resultado.IdCnpj.Replace(" ", "0");

                _logger.LogDebug(
                    "CNPJ Triangulação recuperado: CdTPrd={CdTPrd}, IdCnpj={IdCnpj}, CdFbr={CdFbr}",
                    resultado.CdTPrd, resultado.IdCnpj, resultado.CdFbr);
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao recuperar CNPJ triangulação: CdMercDst={CdMercDst}, DtcSelFtrm={DtcSelFtrm}, LgonFunc={LgonFunc}, IcSim={IcSim}",
                cdMercDst, dtcSelFtrm, lgonFunc, icSim);
            throw;
        }
    }
}
