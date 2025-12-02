using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.FaturamentoPws
{
    public interface IAtualizarPwsService
    {
        Task<AtualizarPwsOutputDto> ExecutarAtualizacaoPwsAsync(AtualizarPwsInputDto input);
    }
}
