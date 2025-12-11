using H1SF.Application.Services.MontaProtocolo;
using System;
using System.Collections.Generic;

namespace H1SF.Application.Services
{
    /// <summary>
    /// Tradução isolada do trecho COBOL 710-00...710-99 para C#.
    /// Arquivo totalmente independente — não mistura trechos de outros módulos.
    /// Métodos de acesso a dados e impressão são virtuals para override/injeção.
    /// </summary>
    public class MontaProtocolo710Service : IMontaProtocoloService
    {
        // Campos COBOL-like (WS / WQ / WI / CB)
        public int WS01NumViasPtd { get; set; }
        public string WS36CdMercDst { get; set; } = string.Empty;
        public string WS36FaseFtrm { get; set; } = string.Empty;

        public string WS01CdSqnPjlPtd { get; set; } = string.Empty;
        public int WS01CdSqnPjlPtdN { get; set; }
        public string CB0002DreCdSqnPjl { get; set; } = string.Empty;

        public int WS01SeqVolPtd { get; set; }
        public int WS01SeqVolPtdImp { get; set; }

        public string WS01Pcl { get; set; } = string.Empty;
        public string WS01LinhaImprAux { get; set; } = string.Empty;
        public string WS01PclDados { get; set; } = string.Empty;

        public int WS01CalcPosAux { get; set; }

        // Campos provenientes do cursor/queries
        protected int WQ01IdVolDetPtd { get; set; }
        protected int WQ01IdVolCmis2DetPtd { get; set; }
        protected int WI01IdVolCmis2DetPtd { get; set; }

        protected int WQ01TotPecaDetPtd { get; set; }
        protected string WQ01NmAcndPrtgPtd { get; set; } = string.Empty;
        protected string WQ01IdCntgConf { get; set; } = string.Empty;

        protected int WQ01Pkg { get; set; }
        protected int WQ01Bulk { get; set; }
        protected int WQ01Minima { get; set; }
        protected string WQ01NmPecaPrtg { get; set; } = string.Empty;

        protected int WS01BulkPkg { get; set; }
        protected int WS01BulkPkgQtd { get; set; }
        protected int WS01BulkPkgRes { get; set; }
        protected int WS01BulkPkgInt { get; set; }

        protected int WS01QtdVolConf { get; set; }
        protected int WS01TotVolConf { get; set; }

        protected int WQ01TotItemDetPtd { get; set; }
        protected string WQ01TotPesoDetPtd { get; set; } = string.Empty;

        protected int WM01QtdAux { get; set; }

        public string WS01PclReset { get; set; } = string.Empty;
        protected string WS01PclDadosReset { get; set; } = string.Empty;

        public MontaProtocolo710Service()
        {
        }

        // Entrada pública que corresponde a 710-00-MONTA-PROTOCOLO
        public virtual void MontaProtocolo()
        {
            // ADD 1 TO WS01-NUM-VIAS-PTD.
            WS01NumViasPtd++;

            if (WS01NumViasPtd == 1)
            {
                // MOVE WS01-CD-SQN-PJL-PTD TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = WS01CdSqnPjlPtd;
            }
            else if (WS01NumViasPtd == 3 && WS36CdMercDst == "D" && WS36FaseFtrm == "2")
            {
                // MOVE '96' TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = "96";
            }
            else
            {
                // ADD 1 TO WS01-CD-SQN-PJL-PTD-N
                WS01CdSqnPjlPtdN++;
                // MOVE WS01-CD-SQN-PJL-PTD TO CB0002-DRE-CD-SQN-PJL
                CB0002DreCdSqnPjl = WS01CdSqnPjlPtd;
                // SUBTRACT 1 FROM WS01-CD-SQN-PJL-PTD-N
                WS01CdSqnPjlPtdN--;
            }

            // 710-10-MONTA-CORPO-INICIO -> PERFORM 715-00-MONTA-CORPO-PROTOCOLO
            MontaCorpoInicio();

            // 710-20-DADOS-DE-DETALHE
            DadosDeDetalhe();

            // 710-99-MONTA-EXIT (retorno implícito)
        }

        // 710-10-MONTA-CORPO-INICIO
        protected virtual void MontaCorpoInicio()
        {
            MontaCorpoProtocolo();
        }

        // 715-00-MONTA-CORPO-PROTOCOLO (stub — override para implementação real)
        protected virtual void MontaCorpoProtocolo()
        {
            WS01SeqVolPtd = 0;
            WS01SeqVolPtdImp = 0;
            WS01Pcl = string.Empty;
            WS01LinhaImprAux = string.Empty;
        }

        // 710-20-DADOS-DE-DETALHE
        protected virtual void DadosDeDetalhe()
        {
            OpenCursor_CSR_SEL_710();
            LoopDetPtd();
            CloseCursor_CSR_SEL_710();

            // Dois PERFORM 545-00-GRAVA-DETALHE-REL
            GravaDetalheRel();
            GravaDetalheRel();

            // SELECT TO_CHAR(TO_NUMBER(:WQ01-QTD-AUX),'99999') INTO :WM01-QTD-AUX FROM DUAL
            WM01QtdAux = ConvertQtdAuxForPrint();
            WS01PclDadosReset = WM01QtdAux.ToString();
            WS01LinhaImprAux = WS01PclReset;
            GravaDetalheRel();
            GravaDetalheRel();
        }

        // Abstrações do cursor SQL (override para implementar acesso real)
        protected virtual void OpenCursor_CSR_SEL_710() { }
        protected virtual IEnumerable<DetailRow> Fetch_CSR_SEL_710() { yield break; }
        protected virtual void CloseCursor_CSR_SEL_710() { }

        // Core do loop DO COBOL 710-40-LOOP-DET-PTD
        private void LoopDetPtd()
        {
            foreach (var row in Fetch_CSR_SEL_710())
            {
                // FETCH ... INTO ...
                WQ01IdVolDetPtd = row.IdVol;
                WQ01IdVolCmis2DetPtd = row.IdVolCmis2;
                WI01IdVolCmis2DetPtd = row.IdVolCmis2;

                // Em implementação C#, Fetch termina o foreach quando não houver mais linhas.

                // ADD 1 TO WS01-SEQ-VOL-PTD
                WS01SeqVolPtd++;
                // ADD 1 TO WS01-SEQ-VOL-PTD-IMP
                WS01SeqVolPtdImp++;

                // IF WS01-SEQ-VOL-PTD GREATER 38
                if (WS01SeqVolPtd > 38)
                {
                    WS01SeqVolPtd = 1;
                    MontaCorpoProtocolo();
                }

                // COMPUTE WS01-CALC-POS-AUX = 1140 + ((WS01-SEQ-VOL-PTD - 1) * 70)
                WS01CalcPosAux = 1140 + ((WS01SeqVolPtd - 1) * 70);

                // Imprime duas linhas
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                // Escolhe ID CMIS2 se presente
                WS01PclDados = (WI01IdVolCmis2DetPtd > -1) ? WQ01IdVolCmis2DetPtd.ToString() : WQ01IdVolDetPtd.ToString();

                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                // SUM ITD_Q_PECA_FTRD INTO WQ01-TOT-PECA-DET-PTD
                WQ01TotPecaDetPtd = GetSumPecasForVolume(WQ01IdVolDetPtd);

                // SELECT INITCAP(LOWER(EA.NM_ACND_PRTG)), EA.ID_CNTG_CONF INTO ...
                var acond = GetAcondicionamentoForVolume(WQ01IdVolDetPtd);
                WQ01NmAcndPrtgPtd = acond?.Nome ?? string.Empty;
                WQ01IdCntgConf = acond?.IdCntgConf ?? string.Empty;

                if (WQ01IdCntgConf == "V")
                {
                    WS01QtdVolConf++;
                    WS01TotVolConf++;
                    QtdVol();
                    continue;
                }

                // SELECT packaging info (pkg/bulk/minima/nome)
                var pkg = GetPackagingForVolume(WQ01IdVolDetPtd);
                WQ01Pkg = pkg?.Pkg ?? 0;
                WQ01Bulk = pkg?.Bulk ?? 0;
                WQ01Minima = pkg?.Minima ?? 1;
                WQ01NmPecaPrtg = pkg?.NomePeca ?? string.Empty;

                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                if (WQ01Bulk > 0)
                {
                    WS01BulkPkg = WQ01Bulk;
                    WS01BulkPkgQtd = SafeDivide(WQ01TotPecaDetPtd, WQ01Bulk);
                }
                else if (WQ01Pkg > 0)
                {
                    WS01BulkPkg = WQ01Pkg;
                    WS01BulkPkgQtd = SafeDivide(WQ01TotPecaDetPtd, WQ01Pkg);
                }
                else
                {
                    WS01BulkPkg = WQ01Minima;
                    WS01BulkPkgQtd = SafeDivide(WQ01TotPecaDetPtd, WQ01Minima);
                }

                // FORMATAÇÃO / TO_CHAR -> simulada
                WQ01Bulk = FormatQtdAuxAsInt(WS01BulkPkgQtd);

                if (WS01BulkPkgRes > 0)
                    WS01BulkPkgInt++;

                WS01QtdVolConf += WS01BulkPkgInt;
                WS01TotVolConf += WS01BulkPkgInt;

                // 710-45-QTD-VOL
                QtdVol();

                // Impressões finais do detalhe conforme COBOL
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WS01PclDados = WQ01NmAcndPrtgPtd;
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WS01PclDados = WQ01Bulk.ToString();
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WQ01TotItemDetPtd = GetCountItemsForVolume(WQ01IdVolDetPtd);
                WS01PclDados = WQ01TotItemDetPtd.ToString();
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WS01PclDados = WQ01TotPecaDetPtd.ToString();
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WQ01TotPesoDetPtd = GetPesoBrutoForVolume(WQ01IdVolDetPtd);
                WS01PclDados = WQ01TotPesoDetPtd;
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();

                WS01PclDados = (WQ01IdCntgConf == "V") ? "Diversos" : WQ01NmPecaPrtg;
                WS01LinhaImprAux = WS01Pcl;
                GravaDetalheRel();
            }
        }

        // 710-45-QTD-VOL
        protected virtual void QtdVol()
        {
            WM01QtdAux = ConvertQtdAuxForPrint();

            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WS01PclDados = WQ01NmAcndPrtgPtd;
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WS01PclDados = WQ01Bulk.ToString();
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WQ01TotItemDetPtd = GetCountItemsForVolume(WQ01IdVolDetPtd);
            WS01PclDados = WQ01TotItemDetPtd.ToString();
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WS01PclDados = WQ01TotPecaDetPtd.ToString();
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WQ01TotPesoDetPtd = GetPesoBrutoForVolume(WQ01IdVolDetPtd);
            WS01PclDados = WQ01TotPesoDetPtd;
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();

            WS01PclDados = (WQ01IdCntgConf == "V") ? "Diversos" : WQ01NmPecaPrtg;
            WS01LinhaImprAux = WS01Pcl;
            GravaDetalheRel();
        }

        // Métodos substituíveis para consultas e utilitários (override para integrar DB / formatação real)
        protected virtual int GetSumPecasForVolume(int idVol) => 0;
        protected virtual (string Nome, string IdCntgConf)? GetAcondicionamentoForVolumeTuple(int idVol) => null;
        protected virtual Acondicionamento GetAcondicionamentoForVolume(int idVol)
        {
            var t = GetAcondicionamentoForVolumeTuple(idVol);
            if (t == null) return null;
            return new Acondicionamento { Nome = t.Value.Nome, IdCntgConf = t.Value.IdCntgConf };
        }
        protected virtual PackagingInfo GetPackagingForVolume(int idVol) => null;
        protected virtual int GetCountItemsForVolume(int idVol) => 0;
        protected virtual string GetPesoBrutoForVolume(int idVol) => "0,000";
        protected virtual int ConvertQtdAuxForPrint() => 0;
        protected virtual int FormatQtdAuxAsInt(int qtd) => qtd;
        protected virtual int SafeDivide(int num, int den) => den == 0 ? 0 : num / den;

        // 545-00-GRAVA-DETALHE-REL (stub — override para escrita/impressão)
        protected virtual void GravaDetalheRel() { /* usar WS01LinhaImprAux, WS01PclDados etc. */ }

        // Tipos de suporte
        protected class DetailRow
        {
            public int IdVol { get; set; }
            public int IdVolCmis2 { get; set; }
        }

        protected class PackagingInfo
        {
            public int Pkg { get; set; }
            public int Bulk { get; set; }
            public int Minima { get; set; }
            public string NomePeca { get; set; } = string.Empty;
        }

        protected class Acondicionamento
        {
            public string Nome { get; set; } = string.Empty;
            public string IdCntgConf { get; set; } = string.Empty;
        }
    }
}