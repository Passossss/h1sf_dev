using H1SF.Application.Services.GravaLançamentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services.MontaItens
{
    public class MontaItensService : IMontaItensService
    {
        // Variáveis COBOL simuladas
        private string SF0001_PTD_CD_MERC_DST;
        private string SF0001_PTD_DTC_SEL_FTRM;
        private string SF0001_PTD_LGON_FUNC;
        private string SF0001_PTD_ID_PTC_DSP;

        private string CC0001_NUMERO;

        private string WQ01_MAX_ITEM;
        private string WQ01_MAX_ITEM_4;

        private string ST80014_Q_ITM_NF;

        private string WS01_NFE_LOCALIZADOR;
        private string ST80014_NFE_LCLR;

        private string SF0002_ITD_RATE_PGT;
        private string ST80014_ID_RATE_PGT;

        private string SF0020_TIP_Q_DIA_PGT;
        private string ST80014_Q_DIA_PGT;

        private readonly IGravaLancamentosCtService gravaLanc;

        public MontaItensService(IGravaLancamentosCtService gravaLanc)
        {
            this.gravaLanc = gravaLanc;

            // Valores default para simulação
            SF0001_PTD_CD_MERC_DST = "";
            SF0001_PTD_DTC_SEL_FTRM = "";
            SF0001_PTD_LGON_FUNC = "";
            SF0001_PTD_ID_PTC_DSP = "";
            CC0001_NUMERO = "";

            WQ01_MAX_ITEM = "0";
            WQ01_MAX_ITEM_4 = "0000";

            WS01_NFE_LOCALIZADOR = "";
            SF0002_ITD_RATE_PGT = "";
            SF0020_TIP_Q_DIA_PGT = "";
        }

        public void ContaItens()
        {
            // PERFORM 840-00-GERA-LANCAMENTOS-CT
            gravaLanc.GravaLancamentosCt();

            // 790-60-CONTA-ITENS
            ExecutaSelectContaItens();

            // INSPECT WQ01-MAX-ITEM CONVERTING SPACES TO ZEROS
            WQ01_MAX_ITEM = string.IsNullOrWhiteSpace(WQ01_MAX_ITEM) ? "0" : WQ01_MAX_ITEM;

            // MOVE WQ01-MAX-ITEM-4 TO ST80014-Q-ITM-NF
            ST80014_Q_ITM_NF = WQ01_MAX_ITEM_4;

            // MOVE WS01-NFE-LOCALIZADOR TO ST80014-NFE-LCLR
            ST80014_NFE_LCLR = WS01_NFE_LOCALIZADOR;

            // INSPECT SF0002-ITD-RATE-PGT CONVERTING SPACES TO ZEROS
            if (string.IsNullOrWhiteSpace(SF0002_ITD_RATE_PGT))
                SF0002_ITD_RATE_PGT = "0";

            // MOVE SF0002-ITD-RATE-PGT TO ST80014-ID-RATE-PGT
            ST80014_ID_RATE_PGT = SF0002_ITD_RATE_PGT;

            // INSPECT SF0020-TIP-Q-DIA-PGT CONVERTING SPACES TO ZEROS
            if (string.IsNullOrWhiteSpace(SF0020_TIP_Q_DIA_PGT))
                SF0020_TIP_Q_DIA_PGT = "0";

            // MOVE SF0020-TIP-Q-DIA-PGT TO ST80014-Q-DIA-PGT
            ST80014_Q_DIA_PGT = SF0020_TIP_Q_DIA_PGT;

            // 790-99 → exit
        }

        private void ExecutaSelectContaItens()
        {
            // SQL do COBOL convertido para string
            string sql = @"
SELECT MAX(ITD_CD_SEQ_ITM)
FROM H1SF.ITD_ITMFATURADO
WHERE ITD_CD_MERC_DST  = @p1
  AND ITD_DTC_SEL_FTRM = @p2
  AND ITD_LGON_FUNC    = @p3
  AND ITD_ID_PTC_DSP   = @p4
  AND ITD_ID_NF        = @p5";

            // Simulação: WQ01_MAX_ITEM recebe um valor qualquer
            WQ01_MAX_ITEM = "0012";
            WQ01_MAX_ITEM_4 = "0012";
        }
    }
}
