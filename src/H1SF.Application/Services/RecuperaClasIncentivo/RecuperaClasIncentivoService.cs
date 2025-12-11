using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class RecuperaClasIncentivoService : IRecuperaClasIncentivoService
    {
        private string WS35_AUX_TS;
        private string WS31_CHV_INC_OK;
        private string WS31_CHV_NTFD_SQL = "100"; // Exemplo
        private string CC0002_MERC_CODIGO_LIMPO;
        private string CC0002_NBM_CODIGO;

        private int WQO1_ORDEM;
        private string CC0009_CMERC_CODIGO;

        private string SF0010_CLS_IC_RDC_BS;
        private string WI01_CLS_IC_RDC_BS;

        private string SF0010_CLS_IC_RDC_PERC;
        private string WI01_CLS_IC_RDC_PERC;

        private string SF0010_CLS_IC_CLS_ATV;
        private string WI01_CLS_IC_CLS_ATV;

        private string CC0004_UF_CODIGO;

        private readonly IRegistraErroTrnsService registraErro;

        public RecuperaClasIncentivoService(IRegistraErroTrnsService registraErro)
        {
            this.registraErro = registraErro;

            WS31_CHV_INC_OK = "N";
        }

        public void RecuperarClasIncentivo()
        {
            // MOVE '525-00-RECUPERA-CLAS-INCENTIVO' TO WS35-AUX-TS
            WS35_AUX_TS = "525-00-RECUPERA-CLAS-INCENTIVO";

            // MOVE 'N' TO WS31-CHV-INC-OK
            WS31_CHV_INC_OK = "N";

            AbrirCursor();

            while (true)
            {
                bool temRegistro = FetchCursor();

                if (!temRegistro)
                    break;

                if (!SelecionaClasse())
                    continue;

                if (RegraFinalIncentivo())
                {
                    WS31_CHV_INC_OK = "S";
                    break;
                }
            }

            FecharCursor();
        }

        // ----------------------------
        // SQL CURSOR: OPEN
        // ----------------------------
        private void AbrirCursor()
        {
            string sql = @"
DECLARE CURSOR CSR_SEL_525 FOR
SELECT 1, A.CMERC_CODIGO
FROM B8CC.COR_CLASSE_MERCADORIA A,
     B8CC.COR_CLASSIFICACAO_MERCADORIA B,
     B8CC.COR_MERCADORIA C
WHERE A.CMERC_CODIGO = B.CMERC_CODIGO(+)
  AND B.MERC_CODIGO = C.MERC_CODIGO(+)
  AND B.DT_INICIO <= SYSDATE
  AND (B.DT_FIM >= SYSDATE OR B.DT_FIM IS NULL)
  AND C.MERC_CODIGO = :CC0002_MERC_CODIGO_LIMPO
UNION ALL
SELECT 2, A.CMERC_CODIGO
FROM B8CC.COR_CLASSE_MERCADORIA A,
     B8CC.COR_CLASSIFICACAO_NBM B,
     B8CC.COR_NBM C
WHERE A.CMERC_CODIGO = B.CMERC_CODIGO(+)
  AND B.NBM_CODIGO = C.NBM_CODIGO(+)
  AND B.DT_INICIO <= SYSDATE
  AND (B.DT_FIM >= SYSDATE OR B.DT_FIM IS NULL)
  AND C.NBM_CODIGO = :CC0002_NBM_CODIGO
ORDER BY 1, 2 DESC";

            // Simulação: cursor aberto
        }

        // ----------------------------
        // SQL CURSOR: FETCH
        // ----------------------------
        private int fetchIndex = 0;

        private bool FetchCursor()
        {
            // Simula o FETCH
            if (fetchIndex == 0)
            {
                WQO1_ORDEM = 1;
                CC0009_CMERC_CODIGO = "123";
            }
            else if (fetchIndex == 1)
            {
                WQO1_ORDEM = 2;
                CC0009_CMERC_CODIGO = "999";
            }
            else
            {
                return false;
            }

            fetchIndex++;
            return true;
        }

        // ----------------------------
        // 525-20-SELECIONA-CLASSE
        // ----------------------------
        private bool SelecionaClasse()
        {
            // SELECT CLS_IC_RDC_BS, CLS_IC_RDC_PERC, CLS_IC_CLS_ATV
            string sql = @"
SELECT CLS_IC_RDC_BS, CLS_IC_RDC_PERC, CLS_IC_CLS_ATV
FROM H1SF.CLS_CLASSE_FISCAL
WHERE CLS_ID_CLS_FSC = :CC0009_CMERC_CODIGO";

            // Simulação de resultados
            WI01_CLS_IC_RDC_BS = "1";
            WI01_CLS_IC_RDC_PERC = "1";
            WI01_CLS_IC_CLS_ATV = "S";

            SF0010_CLS_IC_RDC_BS = WI01_CLS_IC_RDC_BS;
            SF0010_CLS_IC_RDC_PERC = WI01_CLS_IC_RDC_PERC;
            SF0010_CLS_IC_CLS_ATV = WI01_CLS_IC_CLS_ATV;

            bool erro =
                string.IsNullOrWhiteSpace(WI01_CLS_IC_RDC_BS) ||
                string.IsNullOrWhiteSpace(WI01_CLS_IC_RDC_PERC) ||
                string.IsNullOrWhiteSpace(WI01_CLS_IC_CLS_ATV);

            if (erro)
            {
                registraErro.RegistrarErro("Erro classe fiscal", CC0009_CMERC_CODIGO);
                return false;
            }

            // IF SF0010-CLS-IC-CLS-ATV = 'N' → volta ao loop
            if (SF0010_CLS_IC_CLS_ATV == "N")
                return false;

            return true;
        }

        // ----------------------------
        // Regra final de incentivo
        // ----------------------------
        private bool RegraFinalIncentivo()
        {
            bool regra1 = SF0010_CLS_IC_RDC_BS == "S" || SF0010_CLS_IC_RDC_PERC == "S";

            bool regra2 =
                CC0009_CMERC_CODIGO != "01" ||
                (CC0009_CMERC_CODIGO == "01" && CC0004_UF_CODIGO == "SP");

            return regra1 && regra2;
        }

        private void FecharCursor()
        {
            // Simulação CLOSE CURSOR
        }

        public interface IRegistraErroTrnsService
        {
            void RegistrarErro(string v, string cC0009_CMERC_CODIGO);
        }
    }

}
