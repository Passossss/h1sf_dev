using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public interface IPclDanfe
    {
        bool Executar(
        ref string ws35AuxTs,
        ref int ws01IdVia,
        string ws36CdMercDst,
        string ws36FaseFtrm,
        ref string ws01CdSqnPjlNf,
        ref string cb0002DreCdSqnPjl,
        ref int ws01NumPagAux,
        string ws01IcPagAdic,
        Action? perform170DefineViasOrdenacao = null
    );
    }
}
