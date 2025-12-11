using H1SF.Application.Services.DefubeViasOrdenacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class DefinePJjlOutBinP140 : IDefinePJjlOutBinP140

    {
        private IDefineViasOrdenacao _defineViasOrdenacao = null;
        //140-00-DEFINE-PJL-OUTBIN-P
        //Mock
        //Mockado por verificar valor em uma condição utilizando CICS
        public DefinePJjlOutBinP140(IDefineViasOrdenacao defineViasOrdenacao)
        {
            _defineViasOrdenacao = defineViasOrdenacao;
        }

        public void Executar()
        {
            // Lógica do serviço DefinePJjlOutBinP140
        }
    }
}
