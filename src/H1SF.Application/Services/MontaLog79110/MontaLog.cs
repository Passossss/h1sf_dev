using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class MontaLog : IMontaLog
    {
        // Minimal DTOs for inputs/outputs used by the routine.
        // Add fields as needed when integrating into your project.
        public class Sf0001
        {
            public string PtdCdMercDst = "";
            public string PtdDtcSelFtrm = "";
            public string PtdLgonFunc = "";
            public string PtdIdPtcDsp = "";
        }

        public class Cc0001
        {
            public string Numero = "";
            public string NumeroCtb5 = "";
            public string SerieSubserie3 = "";
        }

        public class Cc0002
        {
            public string IdfTextoComplementar = "";
            public string NbmCodigo = "";
            public string CfopCodigo = "";
            public string IdfNum = "";
            public string PrecoUnitarioLog = "";
            public string VlFiscal = "";
            public string VlTributavelIcms = "";
            public string VlIcms = "";
            public string VlImpostoPis = "";
            public string VlImpostoCofins = "";
            public string VlAliqPis5 = "";
            public string VlAliqCofins5 = "";
            public string StaCodigo = "";
            public string StnCodigo = "";
            public string NopCod1 = "";
            public string NopCod2 = "";
            public string VlPisSt = "";
            public string VlCofinsSt = "";
        }

        public class Wq01
        {
            public decimal PesoBrtEmifUnit = 0m; // WQ01-PESO-BRT-EMIF-UNIT
        }

        // Output structure that represents ST80014 fields used in the COBOL section.
        public class St80014
        {
            public string CdAces = "";
            public string CdFbrI = "";
            public string CdSrieNfI = "";
            public string IdNfI = "";
            public string CdTAcnd = "";
            public string IdVol = "";
            public string IdPdd = "";
            public string IdPso = "";
            public string CdDspsPdd = "";
            public string CdStmAtnd = "";
            public string CdModTrspImp = "";
            public string DtcAtndPeca = "";
            public string IdPecaPdd = "";
            public string CdGrPeca = "";
            public string CdFntSpr = "";
            public string CdFntSprAntg = "";
            public string CdFntAtnd = "";
            public string CdTPrecFtrm = "";
            public string CdPrecFrmeFtrm = "";
            public string IdLcacEtqgCli = "";
            public string CdTPgm = "";
            public string CdPgtSppp = "";
            public string CdNumPgm = "";
            public string IdSqnMtg = "";
            public string NmFntAtnd = "";
            public string CdPfo = "";
            public string TxtInfrAdic = "";
            public string IdCcr = "";
            public string IdCcrVrs = "";
            public string IdCcrNumLnh = "";
            public string IdSqnItemPdd = "";
            public string PrecUntInfD = "";
            public string IdLoteFtrmCbl = "";
            public string IcNaczIcpnBt = "";
            public decimal PesoUnitTotR = 0m;
            public string IdClsfFscl = "";
            public string IdEtqRec = "";
            public string IdEtqAcnd = "";
            public string CdSitAtnd = "";
            public string CdFntAtndOrig = "";
            public string CdCfop = "";
            public string IdPercIpi = "";
            public string IdPercIcmsItem = "";
            public string IdPercServ = "";
            public string IdPercSurChrg = "";
            public string IdPercDsctItem = "";
            public string PrecFtrdUntEmif = "";
            public string VttlfscItm = "";
            public string VttlservItem = "";
            public string VttlSurChrgItem = "";
            public string VttlAdcIpiItm = "";
            public string VttlIpiItem = "";
            public string VttlFrtItem = "";
            public string VttlDescItem = "";
            public string VttlBsIcmsItem = "";
            public string VttlIcmsItem = "";
            public string VttlPisItem = "";
            public string VttlCofinsItem = "";
            public string PrecDnPpmUnit = "";
            public string VttlBsIcmsSbsc = "";
            public string VttlCascItem = "";
            public string VttlCmisItem = "";
            public string PrecUntInfD_Out = "";
            public string IdLoteFtrmCblOut = "";
            public string CdSitTribPis = "";
            public string CdSitTribCof = "";
            public string CdNop = "";
            public string IdPrfl = "";
            public string IdCcrConditional = "";
        }

        public class ItemLog
        {
            // only the most used fields are represented here; add more as needed
            public string ItdIdVol = "";
            public string ItdIdPdd = "";
            public string ItdIdPeca = "";
            public string ItdIdEtiqRec = "";
            public string ItdIdEtiqAcnd = "";
            public decimal ItdQPecaFtrd = 0m;
            public string ItdQPecaFtrdCtb = "";
            public decimal ItdVFrt = 0m;
            public decimal ItdPercTxServ = 0m;
            public decimal ItdPercSurChrg = 0m;
            public decimal ItdVTxServ = 0m;
            public decimal ItdVSurChrg = 0m;
            public decimal ItdPercDsct = 0m;
            public string Wq01DescontoItemLog = "";
            public string ItdIdItmDe = "";
            public string ItdIdNfLog = "";
            public decimal ItdVAdcIpi = 0m;
            public string ItdPesoLqdPeca = "";
            public string ItdPesoBrtPeca = "";
            public string ItdIdPrfl = "";
            public decimal ItdVcasRmanOda = 0m;
            public decimal ItdVcasRmanCmis2 = 0m;
            public string ItdIdCascCbl = "";
            public string ItdPrecUsDnCore = "";
            public string ItdPrecUsIcCore = "";
            public string ItdVUsRtFrt = "";
            public string ItdVUsRtSgr = "";
            public string ItdVUsRtOda = "";
            public string ItdVAIChrgApi = "";
            public string ItdVEmgChrgApi = "";
            public string ItdVMktDiscApi = "";
            public string ItdIdFtrApiLog = "";
            public decimal ItdVUsAredCesOda = 0m;
            public string ItdVSurChrgApi = "";
            // fields from B (ITEM_RECOLHIMENTO)
            public string CdSitAtnd = "";
            public string IdPdd_B = "";
            public string IdPso = "";
            public string IdSqnAtndItem = "";
            public string IdSqnOcrFnt = "";
            public string DtcAtndPeca = "";
            public string IdPecaPdd_B = "";
            public string CdGrPeca = "";
            public string CdFntSpr = "";
            public string CdFntSprAntg = "";
            public string CdFntAtnd = "";
            public string CdTPrecFtrm = "";
            public string CdPrecFrmeFtrm = "";
            public string IdLcacEtqgCli = "";
            public string CdTPgm = "";
            public string CdPgtSppp = "";
            public string CdNumPgm = "";
            public string IdSqnMtg = "";
            public string NmFntAtnd = "";
            public string CdPfo = "";
            public string TxtSubHdr = "";
            public string IdCcr = "";
            public string IdCcrVrs = "";
            public string IdCcrNumLnh = "";
            public string IdSqnItemPdd = "";
            public string PrecUntInfD = "";
            public string IdLoteFtrmCbl = "";
            public string IcNaczIcpnBt = "";
        }

        // Returns true when success; false when an error occurred (error message set).
        public bool Executar(
            IDbConnection connection,
            Sf0001 sf0001,
            Cc0001 cc0001,
            Cc0002 cc0002,
            Wq01 wq01,
            out ItemLog itemLog,
            out St80014 st80014,
            out string errorMessage)
        {
            itemLog = new ItemLog();
            st80014 = new St80014();
            errorMessage = null;

            // Initialize / CLEAR ST8001-ST80014 - emulate by resetting the ST80014 object
            st80014 = new St80014();
            st80014.CdAces = "21";

            // Prepare SQL (combined columns per COBOL SELECT). Parameter names use @; adjust for your provider.
            const string sql = @"
SELECT
 A.ITD_ID_VOL,
 A.ITD_ID_PDD,
 A.ITD_ID_PECA,
 A.ITD_ID_ETIQ_REC,
 A.ITD_ID_ETIQ_ACND,
 A.ITD_Q_PECA_FTRD,
 LTRIM(TO_CHAR(A.ITD_Q_PECA_FTRD,'0000000V00')) AS ITD_Q_PECA_FTRD_CTB,
 A.ITD_V_FRT,
 A.ITD_PERC_TX_SERV,
 A.ITD_PERC_SUR_CHRG,
 A.ITD_V_TX_SERV,
 A.ITD_V_SUR_CHRG,
 A.ITD_PERC_DSCT,
 TO_CHAR((DECODE(B.CD_T_PGM,'DELT', A.ITD_V_DSCT_ESP, A.ITD_V_DSCT_ESP*A.ITD_Q_PECA_FTRD)/10000),'9999999V99') AS DESCONTO_ITEM_LOG,
 A.ITD_ID_ITM_DE,
 LTRIM(TO_CHAR(A.ITD_ID_NF,'000000000')) AS ITD_ID_NF_LOG,
 A.ITD_V_ADC_IPI,
 A.ITD_PESO_LQD_PECA,
 A.ITD_PESO_BRT_PECA,
 A.ITD_ID_PRFL,
 A.ITD_V_CAS_RMAN_ODA,
 A.ITD_V_CAS_RMAN_CMIS2,
 A.ITD_ID_CASC_CBL,
 A.ITD_PREC_US_DN_CORE,
 A.ITD_PREC_US_IC_CORE,
 A.ITD_V_US_RT_FRT,
 A.ITD_V_US_RT_SGR,
 A.ITD_V_US_RT_ODA,
 A.ITD_V_AI_CHRG_API,
 A.ITD_V_EMG_CHRG_API,
 A.ITD_V_MKT_DISC_API,
 TRIM(A.ITD_ID_FTR_API) AS ITD_ID_FTR_API,
 NVL(A.ITD_V_US_ARED_CES_ODA,0) AS ITD_V_US_ARED_CES_ODA,
 A.ITD_V_SUR_CHRG_API,
 B.CD_SIT_ATND,
 B.ID_PDD,
 B.ID_PSO,
 B.ID_SQN_ATND_ITEM,
 B.ID_SQN_OCR_FNT,
 TO_CHAR(B.DTC_ATND_PECA, 'DDMMYYYY') AS DTC_ATND_PECA,
 B.ID_PECA_PDD,
 B.CD_GR_PECA,
 B.CD_FNT_SPR,
 B.CD_FNT_SPR_ANTG,
 B.CD_FNT_ATND,
 B.CD_T_PREC_FTRM,
 B.CD_PREC_FRME_FTRM,
 B.ID_LCAC_ETQG_CLI,
 B.CD_T_PGM,
 B.CD_PGT_SPPP,
 B.CD_NUM_PGM,
 B.ID_SQN_MTG,
 B.NM_FNT_ATND,
 B.CD_PFO,
 B.TXT_SUB_HDR,
 B.ID_CCR,
 B.ID_CCR_VRS,
 B.ID_CCR_NUM_LNH,
 B.ID_SQN_ITEM_PDD,
 B.PREC_UNT_INFD,
 B.ID_LOTE_FTRM_CBL,
 B.IC_NACZ_ICPN_BT
FROM H1SF.ITD_ITMFATURADO A,
     H1ST.ITEM_RECOLHIMENTO B
WHERE A.ITD_ID_ETIQ_REC = B.ID_ETIQ_REC
  AND A.ITD_CD_MERC_DST  = :mercDst
  AND A.ITD_DTC_SEL_FTRM = :dtcSelFtrm
  AND A.ITD_LGON_FUNC    = :lgonFunc
  AND A.ITD_ID_PTC_DSP   = :idPtcDsp
  AND A.ITD_ID_NF        = :numero
  AND A.ITD_CD_SEQ_ITM   = :seqItm
  AND ROWNUM = 1";

            var shouldClose = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                shouldClose = true;
            }

            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    AddParam(cmd, "mercDst", sf0001.PtdCdMercDst);
                    AddParam(cmd, "dtcSelFtrm", sf0001.PtdDtcSelFtrm);
                    AddParam(cmd, "lgonFunc", sf0001.PtdLgonFunc);
                    AddParam(cmd, "idPtcDsp", sf0001.PtdIdPtcDsp);
                    AddParam(cmd, "numero", cc0001.Numero);
                    AddParam(cmd, "seqItm", cc0002.IdfNum);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read())
                        {
                            errorMessage = $"*** ERRO - LOG. SYNC. - ITD-ITMFAT. ITEM NF: => {cc0001.Numero}";
                            return false;
                        }

                        // Map many fields; null-safe helpers used
                        itemLog.ItdIdVol = GetString(rdr, 0);
                        itemLog.ItdIdPdd = GetString(rdr, 1);
                        itemLog.ItdIdPeca = GetString(rdr, 2);
                        itemLog.ItdIdEtiqRec = GetString(rdr, 3);
                        itemLog.ItdIdEtiqAcnd = GetString(rdr, 4);
                        itemLog.ItdQPecaFtrd = GetDecimal(rdr, 5);
                        itemLog.ItdQPecaFtrdCtb = GetString(rdr, 6);
                        itemLog.ItdVFrt = GetDecimal(rdr, 7);
                        itemLog.ItdPercTxServ = GetDecimal(rdr, 8);
                        itemLog.ItdPercSurChrg = GetDecimal(rdr, 9);
                        itemLog.ItdVTxServ = GetDecimal(rdr, 10);
                        itemLog.ItdVSurChrg = GetDecimal(rdr, 11);
                        itemLog.ItdPercDsct = GetDecimal(rdr, 12);
                        itemLog.Wq01DescontoItemLog = GetString(rdr, 13);
                        itemLog.ItdIdItmDe = GetString(rdr, 14);
                        itemLog.ItdIdNfLog = GetString(rdr, 15);
                        itemLog.ItdVAdcIpi = GetDecimal(rdr, 16);
                        itemLog.ItdPesoLqdPeca = GetString(rdr, 17);
                        itemLog.ItdPesoBrtPeca = GetString(rdr, 18);
                        itemLog.ItdIdPrfl = GetString(rdr, 19);
                        itemLog.ItdVcasRmanOda = GetDecimal(rdr, 20);
                        itemLog.ItdVcasRmanCmis2 = GetDecimal(rdr, 21);
                        itemLog.ItdIdCascCbl = GetString(rdr, 22);
                        itemLog.ItdPrecUsDnCore = GetString(rdr, 23);
                        itemLog.ItdPrecUsIcCore = GetString(rdr, 24);
                        itemLog.ItdVUsRtFrt = GetString(rdr, 25);
                        itemLog.ItdVUsRtSgr = GetString(rdr, 26);
                        itemLog.ItdVUsRtOda = GetString(rdr, 27);
                        itemLog.ItdVAIChrgApi = GetString(rdr, 28);
                        itemLog.ItdVEmgChrgApi = GetString(rdr, 29);
                        itemLog.ItdVMktDiscApi = GetString(rdr, 30);
                        itemLog.ItdIdFtrApiLog = GetString(rdr, 31);
                        itemLog.ItdVUsAredCesOda = GetDecimal(rdr, 32);
                        itemLog.ItdVSurChrgApi = GetString(rdr, 33);

                        // B.* fields
                        itemLog.CdSitAtnd = GetString(rdr, 34);
                        itemLog.IdPdd_B = GetString(rdr, 35);
                        itemLog.IdPso = GetString(rdr, 36);
                        itemLog.IdSqnAtndItem = GetString(rdr, 37);
                        itemLog.IdSqnOcrFnt = GetString(rdr, 38);
                        itemLog.DtcAtndPeca = GetString(rdr, 39);
                        itemLog.IdPecaPdd_B = GetString(rdr, 40);
                        itemLog.CdGrPeca = GetString(rdr, 41);
                        itemLog.CdFntSpr = GetString(rdr, 42);
                        itemLog.CdFntSprAntg = GetString(rdr, 43);
                        itemLog.CdFntAtnd = GetString(rdr, 44);
                        itemLog.CdTPrecFtrm = GetString(rdr, 45);
                        itemLog.CdPrecFrmeFtrm = GetString(rdr, 46);
                        itemLog.IdLcacEtqgCli = GetString(rdr, 47);
                        itemLog.CdTPgm = GetString(rdr, 48);
                        itemLog.CdPgtSppp = GetString(rdr, 49);
                        itemLog.CdNumPgm = GetString(rdr, 50);
                        itemLog.IdSqnMtg = GetString(rdr, 51);
                        itemLog.NmFntAtnd = GetString(rdr, 52);
                        itemLog.CdPfo = GetString(rdr, 53);
                        itemLog.TxtSubHdr = GetString(rdr, 54);
                        itemLog.IdCcr = GetString(rdr, 55);
                        itemLog.IdCcrVrs = GetString(rdr, 56);
                        itemLog.IdCcrNumLnh = GetString(rdr, 57);
                        itemLog.IdSqnItemPdd = GetString(rdr, 58);
                        itemLog.PrecUntInfD = GetString(rdr, 59);
                        itemLog.IdLoteFtrmCbl = GetString(rdr, 60);
                        itemLog.IcNaczIcpnBt = GetString(rdr, 61);
                    }
                }

                // COBOL checks: if WI01-ID-NF = -1 or SF0002-ITD-ID-NF-LOG invalid -> produce error
                // We try to parse numeric; treat missing/zeros as error
                if (string.IsNullOrWhiteSpace(itemLog.ItdIdNfLog) ||
                    itemLog.ItdIdNfLog.Trim('0').Length == 0)
                {
                    errorMessage = $"*** ERRO - NF COM PROBLEMAS (PFN) NF-I: => {cc0001.Numero}";
                    return false;
                }

                // Get CD_T_ACND from VOLUME_RECOLHIMENTO table
                {
                    const string sql2 = @"
SELECT CD_T_ACND
  FROM H1ST.VOLUME_RECOLHIMENTO
 WHERE ID_VOL = :idVol
   AND ROWNUM = 1";
                    using (var cmd2 = connection.CreateCommand())
                    {
                        cmd2.CommandText = sql2;
                        AddParam(cmd2, "idVol", itemLog.ItdIdVol);
                        var val = cmd2.ExecuteScalar();
                        if (val == null || val == DBNull.Value)
                        {
                            errorMessage = $"*** ERRO - LOG. SYNC. - VOL. RECOL. VOLUME: => {itemLog.ItdIdVol}";
                            return false;
                        }
                        st80014.CdTAcnd = Convert.ToString(val);
                    }
                }

                // INSPECT ... CONVERTING SPACES TO ZEROS -> emulate:
                itemLog.ItdIdEtiqRec = (itemLog.ItdIdEtiqRec ?? "").Replace(' ', '0');
                itemLog.ItdPesoLqdPeca = (itemLog.ItdPesoLqdPeca ?? "").Replace(' ', '0');
                itemLog.ItdPesoBrtPeca = (itemLog.ItdPesoBrtPeca ?? "").Replace(' ', '0');
                itemLog.ItdIdPrfl = (itemLog.ItdIdPrfl ?? "").Replace(' ', '0');
                itemLog.ItdPrecUsDnCore = (itemLog.ItdPrecUsDnCore ?? "").Replace(' ', '0');
                itemLog.ItdPrecUsIcCore = (itemLog.ItdPrecUsIcCore ?? "").Replace(' ', '0');
                itemLog.ItdVUsRtFrt = (itemLog.ItdVUsRtFrt ?? "").Replace(' ', '0');
                itemLog.ItdVUsRtSgr = (itemLog.ItdVUsRtSgr ?? "").Replace(' ', '0');
                itemLog.ItdVUsRtOda = (itemLog.ItdVUsRtOda ?? "").Replace(' ', '0');
                itemLog.TxtSubHdr = (itemLog.TxtSubHdr ?? "").Replace(' ', '0');
                // many more INSPECTs in original --- add as needed

                // Query SPP_PROGRAMAS to set SF0008-SPP-ID-CT-CTBL (if any). Emulate: if missing -> empty, if 'DELT' set => 3
                {
                    const string sql3 = @"
SELECT SPP_ID_CT_CTBL
  FROM H1SF.SPP_PROGRAMAS
 WHERE SPP_CD_T_PGM = RTRIM(:cdTpgm)
   AND SPP_CD_NUM_PGM = :numPgm
   AND ROWNUM = 1";
                    using (var cmd3 = connection.CreateCommand())
                    {
                        cmd3.CommandText = sql3;
                        AddParam(cmd3, "cdTpgm", itemLog.CdTPgm ?? "");
                        AddParam(cmd3, "numPgm", itemLog.CdNumPgm ?? "");
                        var v = cmd3.ExecuteScalar();
                        if (v == null || v == DBNull.Value)
                            st80014.IdCcrConditional = ""; // reuse field for SPP id
                        else
                            st80014.IdCcrConditional = Convert.ToString(v);
                    }

                    if (string.Equals(itemLog.CdTPgm, "DELT", StringComparison.OrdinalIgnoreCase))
                        st80014.IdCcrConditional = "3";
                }

                // Compute WS01-PESO-AUX ROUNDED = WQ01-PESO-BRT-EMIF-UNIT * WS01-Q-PECA-AUX
                // COBOL used numeric fields; here we take inputs: wq01.PesoBrtEmifUnit and itemLog.ItdQPecaFtrd (or WS01-Q-PECA-AUX)
                decimal qPecaAux = itemLog.ItdQPecaFtrd;
                if (qPecaAux == 0m) qPecaAux = 0m;
                var pesoAux = wq01.PesoBrtEmifUnit * qPecaAux;
                st80014.PesoUnitTotR = pesoAux;

                // Populate ST80014 fields from itemLog and CC0002 / CC0001 where appropriate
                st80014.CdFbrI = GetSafe(cc0001.SerieSubserie3);
                st80014.CdSrieNfI = GetSafe(cc0001.SerieSubserie3);
                st80014.IdNfI = (itemLog.ItdIdNfLog ?? "").Replace(' ', '0');

                st80014.IdVol = itemLog.ItdIdVol;
                st80014.IdPdd = itemLog.ItdIdPdd;
                st80014.IdPso = itemLog.IdPso;
                st80014.CdDspsPdd = itemLog.CdGrPeca;
                st80014.CdStmAtnd = itemLog.CdSitAtnd;
                st80014.CdModTrspImp = itemLog.CdFntAtnd;
                st80014.DtcAtndPeca = itemLog.DtcAtndPeca;

                // Convert CC0002 fields where COBOL INSPECT needed
                st80014.IdClsfFscl = (cc0002.NbmCodigo ?? "").Replace(' ', '0');
                st80014.IdEtqRec = (itemLog.ItdIdEtiqRec ?? "").Replace(' ', '0');
                st80014.IdEtqAcnd = (itemLog.ItdIdEtiqAcnd ?? "").Replace(' ', '0');
                st80014.CdSitAtnd = itemLog.CdSitAtnd;

                st80014.CdFntAtnd = itemLog.CdFntAtnd;
                if (string.Equals(itemLog.IcNaczIcpnBt, "S", StringComparison.OrdinalIgnoreCase))
                    st80014.CdFntAtndOrig = "Y305";
                else
                    st80014.CdFntAtndOrig = itemLog.CdFntAtnd;

                st80014.CdCfop = (cc0002.CfopCodigo ?? "").Replace(' ', '0');

                // Move many monetary/tax values to ST80014 fields (string->string as kept in COBOL)
                st80014.IdPercIpi = GetSafe(cc0002.VlAliqPis5);
                st80014.IdPercIcmsItem = GetSafe(cc0002.NbmCodigo); // placeholder mapping
                st80014.IdPercServ = GetSafe(itemLog.ItdPercTxServ.ToString(CultureInfo.InvariantCulture));
                st80014.IdPercSurChrg = GetSafe(itemLog.ItdPercSurChrg.ToString(CultureInfo.InvariantCulture));
                st80014.IdPercDsctItem = GetSafe(itemLog.ItdPercDsct.ToString(CultureInfo.InvariantCulture));
                st80014.PrecFtrdUntEmif = GetSafe(cc0002.PrecoUnitarioLog);
                st80014.VttlfscItm = GetSafe(cc0002.VlFiscal);
                st80014.VttlservItem = GetSafe(itemLog.ItdVTxServ.ToString(CultureInfo.InvariantCulture));
                st80014.VttlSurChrgItem = GetSafe(itemLog.ItdVSurChrg.ToString(CultureInfo.InvariantCulture));
                st80014.VttlAdcIpiItm = GetSafe(itemLog.ItdVAdcIpi.ToString(CultureInfo.InvariantCulture));

                // Move other totals/flags
                st80014.VttlFrtItem = GetSafe(itemLog.ItdVFrt.ToString(CultureInfo.InvariantCulture));
                st80014.VttlDescItem = GetSafe(itemLog.Wq01DescontoItemLog);

                st80014.VttlBsIcmsItem = GetSafe(cc0002.VlTributavelIcms);
                st80014.VttlIcmsItem = GetSafe(cc0002.VlIcms);
                st80014.VttlPisItem = GetSafe(cc0002.VlImpostoPis);
                st80014.VttlCofinsItem = GetSafe(cc0002.VlImpostoCofins);
                st80014.PrecDnPpmUnit = GetSafe(sr0004GetPlaceholder()); // SR0004.PREC-US-DN-PPM-UNIT was fetched earlier in COBOL; placeholder here

                // carry-through some flags from CC0002
                st80014.VttlBsIcmsSbsc = GetSafe(cc0002.VlTributavelIcms);
                st80014.VttlCascItem = GetSafe(itemLog.ItdVcasRmanOda.ToString(CultureInfo.InvariantCulture));
                st80014.VttlCmisItem = GetSafe(itemLog.ItdVcasRmanCmis2.ToString(CultureInfo.InvariantCulture));
                st80014.PrecUntInfD_Out = GetSafe(itemLog.PrecUntInfD);

                st80014.IdLoteFtrmCblOut = GetSafe(itemLog.IdLoteFtrmCbl);

                st80014.CdSitTribPis = GetSafe(cc0002.StaCodigo);
                st80014.CdSitTribCof = GetSafe(cc0002.StnCodigo);

                // NOP code building
                st80014.CdNop = (cc0002.NopCod1 ?? "") + (cc0002.NopCod2 ?? "");

                // Convert and trim ID-PRFL
                st80014.IdPrfl = (itemLog.ItdIdPrfl ?? "").Replace(' ', '0');

                // Handle CCR conditional moves
                if (!string.IsNullOrEmpty(itemLog.IdCcr) && itemLog.IdCcr != "-1")
                    st80014.IdCcr = itemLog.IdCcr;
                else
                    st80014.IdCcr = "";

                // Finalize success
                return true;
            }
            finally
            {
                if (shouldClose && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        // ---------- helpers ----------
        static void AddParam(IDbCommand cmd, string name, object val)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name.StartsWith(":") ? name : ":" + name; // Oracle style in COBOL; most ADO.NET providers map both
            p.Value = val ?? (object)DBNull.Value;
            cmd.Parameters.Add(p);
        }

        static string GetString(IDataRecord r, int idx)
        {
            if (r.IsDBNull(idx)) return null;
            return r.GetValue(idx).ToString();
        }

        static decimal GetDecimal(IDataRecord r, int idx)
        {
            if (r.IsDBNull(idx)) return 0m;
            var v = r.GetValue(idx);
            if (v is decimal d) return d;
            if (v is double db) return Convert.ToDecimal(db);
            if (v is float f) return Convert.ToDecimal(f);
            if (v is int i) return i;
            if (decimal.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal res)) return res;
            return 0m;
        }

        static string GetSafe(string s) => s ?? string.Empty;

        static string sr0004GetPlaceholder() => string.Empty; // placeholder for SR0004-PREC-US-DN-PPM-UNIT - integrate real value when available
    }
}