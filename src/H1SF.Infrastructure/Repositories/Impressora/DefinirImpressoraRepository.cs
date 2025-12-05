using H1SF.Domain.Entities.Faturamento;
using H1SF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Infrastructure.Repositories
{
    public class DefinirImpressoraRepository : IDefinirImpressoraRepository
    {
        private readonly ApplicationDbContext _context;

        public DefinirImpressoraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> ObterIdTipoRecolhimentoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
        {
            // Corresponde à primeira query SQL (com subquery)
            return await _context.ItensFaturados
                .Where(x => x.ItdCdMercDst == cdMercDst &&
                           x.ItdDtcSelFtrm == dtcSelFtrm &&
                           x.ItdLgonFunc == lgonFunc)
                .Select(x => x.ItdCdTRec)
                .FirstOrDefaultAsync();
        }

        public async Task<SelecaoFaturamento?> ObterSelecaoFaturamentoAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc)
        {
            // Corresponde à segunda query SQL
            return await _context.SelecoesFaturamento
                .Where(x => x.SftCdMercDst == cdMercDst &&
                           x.SftDtcSelFtrm == dtcSelFtrm &&
                           x.SftLgonFunc == lgonFunc)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> ObterNomeImpressoraAsync(int idImpressora)
        {
            // Corresponde à terceira query SQL
            return await _context.Impressoras
                .Where(x => x.IdImpr == idImpressora)
                .Select(x => x.NmImprAix)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ValidarImpressoraExisteAsync(int idImpressora)
        {
            return await _context.Impressoras
                .AnyAsync(x => x.IdImpr == idImpressora);
        }
    }
}
