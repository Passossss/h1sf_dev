using H1SF.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public interface IImpressoraService
    {
        Task<DefinirImpressoraOutputDto> DefinirImpressoraAsync(DefinirImpressoraInputDto input);
        Task<int?> ObterTipoRecolhimentoAsync(int cdMercDst, DateTime dtcSelFtrm, string lgonFunc);
    }
}
