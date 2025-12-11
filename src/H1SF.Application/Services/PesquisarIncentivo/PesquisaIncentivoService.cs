using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class PesquisaIncentivoService : IPesquisaIncentivoService
    {
        private string WS35_AUX_TS;

        private int WS32_IND_AUX_INC;
        private string WS01_CLSF_FSCL_AUX_12;

        private string WS01_CLSF_FSCL_AUX_9_10;
        private string WS01_CLSF_FSCL_AUX_7_8;
        private string WS01_CLSF_FSCL_AUX_5_6;
        private string WS01_CLSF_FSCL_AUX_3_4;
        private string WS01_CLSF_FSCL_AUX_1_2;

        private string WQ01_CLSF_FSC;

        private string WS31_CHV_INC_OK;

        private string CC0002_NBM_CODIGO;

        private readonly IRecuperaClasIncentivoService recuperaClasService;

        public PesquisaIncentivoService(IRecuperaClasIncentivoService recuperaClasService)
        {
            this.recuperaClasService = recuperaClasService;

            WS31_CHV_INC_OK = "N";
            WS32_IND_AUX_INC = 0;
        }

        public void PesquisarIncentivo()
        {
            // MOVE '150-00-PESQUISA-INCENTIVO' TO WS35-AUX-TS
            WS35_AUX_TS = "150-00-PESQUISA-INCENTIVO";

            // MOVE ZEROS TO WS32-IND-AUX-INC
            WS32_IND_AUX_INC = 0;

            // MOVE CC0002-NBM-CODIGO TO WS01-CLSF-FSCL-AUX-12
            WS01_CLSF_FSCL_AUX_12 = CC0002_NBM_CODIGO;

            // -------------------------
            // 150-10-LOOP-BUSCA-INC
            // -------------------------
            while (true)
            {
                WS32_IND_AUX_INC++;

                if (WS32_IND_AUX_INC > 1)
                {
                    if (WS32_IND_AUX_INC == 2)
                        WS01_CLSF_FSCL_AUX_9_10 = string.Empty;

                    if (WS32_IND_AUX_INC == 3)
                        WS01_CLSF_FSCL_AUX_7_8 = string.Empty;

                    if (WS32_IND_AUX_INC == 4)
                        WS01_CLSF_FSCL_AUX_5_6 = string.Empty;

                    if (WS32_IND_AUX_INC == 5)
                        WS01_CLSF_FSCL_AUX_3_4 = string.Empty;

                    if (WS32_IND_AUX_INC == 6)
                        WS01_CLSF_FSCL_AUX_1_2 = string.Empty;
                }

                // MOVE WS01-CLSF-FSCL-AUX-12 TO WQ01-CLSF-FSC
                WQ01_CLSF_FSC = WS01_CLSF_FSCL_AUX_12;

                // PERFORM 525-00-RECUPERA-CLAS-INCENTIVO
                recuperaClasService.RecuperarClasIncentivo();

                // IF WS31-CHV-INC-OK = 'S' GO TO EXIT
                if (WS31_CHV_INC_OK == "S")
                    return;

                // IF WS32-IND-AUX-INC < 6 → loop novamente
                if (WS32_IND_AUX_INC < 6)
                    continue;

                // Se chegou aqui, sai
                break;
            }

            // 150-99-PESQUISA-EXIT
            return;
        }
    }

}
