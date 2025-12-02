using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace H1SF.Infrastructure.Repositories.FaturamentoPws
{
    public class AtualizarPwsRepository : IAtualizarPwsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AtualizarPwsRepository> _logger;

        public AtualizarPwsRepository(ApplicationDbContext context, ILogger<AtualizarPwsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeveExecutarAtualizacaoAsync(string? sftIcNaczIcpnBt, string? sftCdTRec)
        {
            if (sftIcNaczIcpnBt == "S" && sftCdTRec != "OT")
            {
                _logger.LogInformation("Atualização PWS não executada: Condição de bypass atendida");
                return false;
            }
            return true;
        }

        public async Task<List<ItemFaturadoAgrupadoDto>> ObterItensAgrupadosAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm)
        {
            var query = _context.ItensFaturados
                .Where(x => x.ItdCdMercDst == cdMercDst &&
                           x.ItdDtcSelFtrm == dtcSelFtrm &&
                           x.ItdLgonFunc == lgonFunc);

            if (faseFtrm == "1")
            {
                query = query.Where(x => x.ItdIdNfRef == null);
            }
            else if (faseFtrm == "2")
            {
                query = query.Where(x => x.ItdIdNfRef != null);
            }

            return await query
                .GroupBy(x => new { x.ItdIdCli, x.ItdIdVol, x.ItdIdEtiqRec })
                .Select(g => new ItemFaturadoAgrupadoDto
                {
                    ItdIdCli = g.Key.ItdIdCli,
                    ItdIdVol = g.Key.ItdIdVol,
                    ItdIdEtiqRec = g.Key.ItdIdEtiqRec,
                    SomaQuantidade = g.Sum(x => x.ItdQPecaFtrd)
                })
                .ToListAsync();
        }

        public async Task<bool> AtualizarItemRecolhimentoAsync(int idEtiqRec, int qPecaRec)
        {
            var item = await _context.ItensRecolhimento
                .FirstOrDefaultAsync(x => x.IdEtiqRec == idEtiqRec && x.QPecaRec == qPecaRec);

            if (item == null) return false;

            item.DtcFnlItem = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> ObterSomaQuantidadesItemAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm, int idEtiqRec)
        {
            var query = _context.ItensFaturados
                .Where(x => x.ItdCdMercDst == cdMercDst &&
                           x.ItdDtcSelFtrm == dtcSelFtrm &&
                           x.ItdLgonFunc == lgonFunc &&
                           x.ItdIdEtiqRec == idEtiqRec);

            if (faseFtrm == "1")
            {
                query = query.Where(x => x.ItdIdNfRef == null);
            }
            else if (faseFtrm == "2")
            {
                query = query.Where(x => x.ItdIdNfRef != null);
            }

            return await query.SumAsync(x => (int?)x.ItdQPecaFtrd);
        }

        public async Task<List<VolumeInfoDto>> ObterVolumesDistintosAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm)
        {
            var query = _context.ItensFaturados
                .Where(x => x.ItdCdMercDst == cdMercDst &&
                           x.ItdDtcSelFtrm == dtcSelFtrm &&
                           x.ItdLgonFunc == lgonFunc);

            if (faseFtrm == "1")
            {
                query = query.Where(x => x.ItdIdNfRef == null);
            }
            else if (faseFtrm == "2")
            {
                query = query.Where(x => x.ItdIdNfRef != null);
            }

            return await query
                .Select(x => new VolumeInfoDto
                {
                    ItdIdMtz = x.ItdIdMtz,
                    ItdIdCli = x.ItdIdCli,
                    ItdIdVol = x.ItdIdVol,
                    ItdIdNf = x.ItdIdNf,
                    ItdCdModTrsp = x.ItdCdModTrsp
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AtualizarIdDocumentoFiscalAsync(int idVol, string idNf)
        {
            var volume = await _context.VolumesRecolhimento
                .FirstOrDefaultAsync(x => x.IdVol == idVol);

            if (volume == null) return false;

            volume.IdDctFscl = idNf;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarVolumeFaturadoAsync(int idVol)
        {
            var todosFinalizados = await VerificarTodosItensFinalizadosAsync(idVol);
            if (!todosFinalizados) return false;

            var volume = await _context.VolumesRecolhimento
                .FirstOrDefaultAsync(x => x.IdVol == idVol);

            if (volume == null) return false;

            volume.IcVolFtrd = "S";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerificarTodosItensFinalizadosAsync(int idVol)
        {
            var existeItemNaoFinalizado = await _context.ItensVolumeRecolhimento
                .Join(_context.ItensRecolhimento,
                    ivr => ivr.IdEtiqRec,
                    ir => ir.IdEtiqRec,
                    (ivr, ir) => new { ivr, ir })
                .Where(x => x.ivr.IdVol == idVol && x.ir.DtcFnlItem == null)
                .AnyAsync();

            return !existeItemNaoFinalizado;
        }
    }
}
