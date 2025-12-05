using H1SF.Domain.Entities.Protocolo;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace H1SF.Infrastructure.Repositories.Protocolo;

/// <summary>
/// 500-00-LE-PROTOCOLO - Lê protocolos de despacho
/// Linhas COBOL: 3278-3285 (chama 700-00 -> 705-00-MONTA-DANFE linhas 5270-5330)
/// </summary>
public class ProtocoloRepository : IProtocoloRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProtocoloRepository> _logger;

    public ProtocoloRepository(
        ApplicationDbContext context,
        ILogger<ProtocoloRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 705-00-MONTA-DANFE
    /// EXEC SQL DECLARE CSR_SEL_A CURSOR FOR
    ///          SELECT PTD_CD_MERC_DST, PTD_DTC_SEL_FTRM, PTD_LGON_FUNC, PTD_CD_SEQ,
    ///                 PTD_CD_T_REC, PTD_CD_T_MTZ, PTD_ID_MTZ, PTD_ID_CLI,
    ///                 PTD_ID_PTC_DSP, LTRIM(TO_CHAR(...)), TO_CHAR(PTD_DTC_PTC_DSP,...),
    ///                 PTD_CD_PGT_FRT, DECODE(PTD_CD_TRSR,'-',NULL,PTD_CD_TRSR),
    ///                 [peso/valor formatados], PTD_IC_DSP_IMPS
    ///          FROM   H1SF.PTD_PROTODSP
    ///          WHERE  PTD_CD_MERC_DST = :WQ02-CD-MERC-DST
    ///          AND    PTD_DTC_SEL_FTRM = :WQ02-DTC-SEL-FTRM
    ///          AND    PTD_LGON_FUNC = :WQ02-LGON-FUNC
    ///          AND    PTD_IC_DSP_IMPS = :WQ01-N
    ///          ORDER BY PTD_CD_MERC_DST, PTD_DTC_SEL_FTRM, PTD_LGON_FUNC, ...
    /// </summary>
    public async Task<List<DadosProtocolo>> ListarProtocolosNaoImpressosAsync(
        string cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc)
    {
        try
        {
            // COBOL: MOVE 'N' TO WQ01-N (busca protocolos não impressos)
            const string icNaoImpresso = "N";

            var protocolos = await _context.ProtocolosDespacho
                .Where(p => p.PtdCdMercDst == cdMercDst &&
                           p.PtdDtcSelFtrm == dtcSelFtrm &&
                           p.PtdLgonFunc == lgonFunc &&
                           p.PtdIcDspImps == icNaoImpresso)
                .OrderBy(p => p.PtdCdMercDst)
                .ThenBy(p => p.PtdDtcSelFtrm)
                .ThenBy(p => p.PtdLgonFunc)
                .ThenBy(p => p.PtdCdTRec)
                .ThenBy(p => p.PtdCdTMtz)
                .ThenBy(p => p.PtdIdMtz)
                .ThenBy(p => p.PtdIdCli)
                .ThenBy(p => p.PtdIdPtcDsp)
                .Select(p => new DadosProtocolo
                {
                    PtdCdMercDst = p.PtdCdMercDst,
                    PtdDtcSelFtrm = p.PtdDtcSelFtrm,
                    PtdLgonFunc = p.PtdLgonFunc,
                    PtdCdSeq = p.PtdCdSeq,
                    PtdCdTRec = p.PtdCdTRec,
                    PtdCdTMtz = p.PtdCdTMtz,
                    PtdIdMtz = p.PtdIdMtz,
                    PtdIdCli = p.PtdIdCli,
                    PtdIdPtcDsp = p.PtdIdPtcDsp,
                    
                    // COBOL: LTRIM(TO_CHAR(TO_NUMBER(PTD_ID_PTC_DSP),'9,999,999'))
                    PtdIdPtcDspFormatado = p.PtdIdPtcDsp.TrimStart(),
                    
                    // COBOL: TO_CHAR(PTD_DTC_PTC_DSP,'DD/MM/YYYY - HH24') || 'h' || TO_CHAR(PTD_DTC_PTC_DSP,'MI')
                    PtdDtcPtcDspFormatado = p.PtdDtcPtcDsp != null
                        ? p.PtdDtcPtcDsp.Value.ToString("dd/MM/yyyy - HH") + "h" + p.PtdDtcPtcDsp.Value.ToString("mm")
                        : string.Empty,
                    
                    PtdCdPgtFrt = p.PtdCdPgtFrt,
                    
                    // COBOL: DECODE(PTD_CD_TRSR,'-',NULL,PTD_CD_TRSR)
                    PtdCdTrsr = p.PtdCdTrsr == "-" ? null : p.PtdCdTrsr,
                    
                    // COBOL: TO_NUMBER(PTD_PESO_TTL_LQD)/1000 (converte gramas para Kg)
                    PesoLiquidoKg = p.PtdPesoTtlLqd.HasValue ? p.PtdPesoTtlLqd.Value / 1000m : 0,
                    PesoBrutoKg = p.PtdPesoTtlBrt.HasValue ? p.PtdPesoTtlBrt.Value / 1000m : 0,
                    PesoEmbalagemKg = (p.PtdPesoTtlBrt.HasValue && p.PtdPesoTtlLqd.HasValue)
                        ? (p.PtdPesoTtlBrt.Value - p.PtdPesoTtlLqd.Value) / 1000m
                        : 0,
                    
                    QuantidadeVolumes = p.PtdQTtlVol ?? 0,
                    
                    // COBOL: TO_NUMBER(PTD_V_TTL_MRCD)/100 (converte centavos para reais)
                    ValorTotalReais = p.PtdVTtlMrcd.HasValue ? p.PtdVTtlMrcd.Value / 100m : 0,
                    
                    PtdIcDspImps = p.PtdIcDspImps
                })
                .ToListAsync();

            // Formatar campos conforme COBOL após carregar do banco
            foreach (var proto in protocolos)
            {
                // COBOL: LTRIM(TO_CHAR(TO_NUMBER(PTD_PESO_TTL_LQD)/1000,'9,999,999,990.000') || 'Kg')
                proto.PesoLiquidoFormatado = proto.PesoLiquidoKg.ToString("N3") + "Kg";
                proto.PesoBrutoFormatado = proto.PesoBrutoKg.ToString("N3") + "Kg";
                proto.PesoEmbalagemFormatado = proto.PesoEmbalagemKg.ToString("N3") + "Kg";
                
                // COBOL: LTRIM(TO_CHAR(PTD_Q_TTL_VOL))
                proto.QuantidadeVolumesFormatado = proto.QuantidadeVolumes.ToString().TrimStart();
                
                // COBOL: 'R$' || TO_CHAR(TO_NUMBER(PTD_V_TTL_MRCD)/100,'99,999,999,990.00')
                proto.ValorTotalFormatado = "R$" + proto.ValorTotalReais.ToString("N2");
            }

            _logger.LogDebug(
                "Protocolos não impressos encontrados: {Count}",
                protocolos.Count);

            return protocolos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao listar protocolos: CdMercDst={CdMercDst}, DtcSelFtrm={DtcSelFtrm}, LgonFunc={LgonFunc}",
                cdMercDst, dtcSelFtrm, lgonFunc);
            throw;
        }
    }

    /// <summary>
    /// 705-00-MONTA-DANFE (linhas 5364-5377)
    /// EXEC SQL UPDATE H1SF.PTD_PROTODSP
    ///          SET PTD_IC_DSP_IMPS = 'S'
    ///          WHERE PTD_CD_MERC_DST = :WQ02-CD-MERC-DST
    ///          AND PTD_DTC_SEL_FTRM = :WQ02-DTC-SEL-FTRM
    ///          AND PTD_LGON_FUNC = :WQ02-LGON-FUNC
    ///          AND PTD_CD_T_REC = :SF0001-PTD-CD-T-REC
    ///          AND PTD_CD_T_MTZ = :SF0001-PTD-CD-T-MTZ
    ///          AND PTD_ID_MTZ = :SF0001-PTD-ID-MTZ
    ///          AND PTD_ID_CLI = :SF0001-PTD-ID-CLI
    ///          AND PTD_ID_PTC_DSP = :SF0001-PTD-ID-PTC-DSP
    /// </summary>
    public async Task AtualizarProtocoloComoImpressoAsync(
        string cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc,
        int cdTRec,
        int cdTMtz,
        int idMtz,
        int idCli,
        string idPtcDsp)
    {
        try
        {
            var protocolo = await _context.ProtocolosDespacho
                .Where(p => p.PtdCdMercDst == cdMercDst &&
                           p.PtdDtcSelFtrm == dtcSelFtrm &&
                           p.PtdLgonFunc == lgonFunc &&
                           p.PtdCdTRec == cdTRec &&
                           p.PtdCdTMtz == cdTMtz &&
                           p.PtdIdMtz == idMtz &&
                           p.PtdIdCli == idCli &&
                           p.PtdIdPtcDsp == idPtcDsp)
                .FirstOrDefaultAsync();

            if (protocolo != null)
            {
                // COBOL: SET PTD_IC_DSP_IMPS = 'S'
                protocolo.PtdIcDspImps = "S";
                await _context.SaveChangesAsync();

                _logger.LogDebug(
                    "Protocolo marcado como impresso: IdPtcDsp={IdPtcDsp}",
                    idPtcDsp);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao atualizar protocolo como impresso: IdPtcDsp={IdPtcDsp}",
                idPtcDsp);
            throw;
        }
    }

    /// <summary>
    /// 705-00-MONTA-DANFE (linhas 5355-5363)
    /// EXEC SQL SELECT 1
    ///          FROM H1SF.ITD_ITMFATURADO
    ///          WHERE ITD_CD_MERC_DST = :SF0001-PTD-CD-MERC-DST
    ///          AND ITD_DTC_SEL_FTRM = :SF0001-PTD-DTC-SEL-FTRM
    ///          AND ITD_LGON_FUNC = :SF0001-PTD-LGON-FUNC
    ///          AND ITD_ID_PTC_DSP = :SF0001-PTD-ID-PTC-DSP
    ///          AND ROWNUM = 1
    /// </summary>
    public async Task<bool> ExistemItensFaturadosAsync(
        string cdMercDst,
        DateTime dtcSelFtrm,
        string lgonFunc,
        string idPtcDsp)
    {
        try
        {
            var existe = await _context.ItensFaturados
                .Where(i => i.ItdCdMercDst == cdMercDst &&
                           i.ItdDtcSelFtrm == dtcSelFtrm &&
                           i.ItdLgonFunc == lgonFunc &&
                           i.ItdIdPtcDsp == idPtcDsp)
                .AnyAsync();

            return existe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao verificar itens faturados: IdPtcDsp={IdPtcDsp}",
                idPtcDsp);
            throw;
        }
    }
}
