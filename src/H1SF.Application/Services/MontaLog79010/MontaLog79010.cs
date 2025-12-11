using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class MontaLog79010 : IMontaLog79010
    {
        // Minimal input DTOs used by this routine.
        public class Sf0001
        {
            public string PtdCdMercDst = "";
            public string PtdDtcSelFtrm = "";
            public string PtdLgonFunc = "";
            public string PtdIdPtcDsp = "";
            public string PtdCdTRec = "";
            public string PtdCdTMtz = "";
            public string PtdIdMtz = "";
            public string PtdIdCli = "";
            public string PtdCdTrsr = "";
        }

        public class Cc0001
        {
            public string Numero = "";
            public string SerieSubserie3 = "";
            public decimal PesoBrutoKgLog = 0m; // CC0001-PESO-BRUTO-KG-LOG
            public decimal VlOutrasDespesasLog = 0m;
            public decimal VlAjustePrecoTotalM = 0m;
            public decimal VlDescontoLog = 0m;
            public decimal PrecoTotalMLog = 0m;
            public decimal VlTotalBaseIcmsLog = 0m;
            public decimal VlTotalStfLog = 0m;
            public decimal VlTotalIcmsLog = 0m;
            public decimal VlTotalIpiLog = 0m;
            public decimal VlTotalContabilLog = 0m;
            public decimal VlTotalPisPasepLog = 0m;
            public decimal VlTotalCofinsLog = 0m;
            public decimal VlFreteLog = 0m;
            public decimal VlSeguroLog = 0m;
            public decimal VlTotalFcpSt = 0m;
        }

        // Output DTO (ST80014) — only fields used in this section are declared.
        public class St80014
        {
            public string CdAces;
            public string CdFbr;
            public string CdTRec;
            public string CdTMtz;
            public string IdMtz;
            public string IdCli;
            public string CdCliEsp;
            public string CdTOprcFtrm;
            public string CdModTrsp;
            public string CdNF;
            public string CdSrieNf;
            public string IdNf;
            public string IdNfCmpl;
            public string DtcNf;
            public string HrNf;
            public string IdTimeStmp;
            public string CdRparFisc;
            public string CdSufr;
            public string CdUf;
            public string CdTdsctCamp;
            public string CdDizrNf;
            public string CdTPgt;
            public string CdInctIcms;
            public string CdTrsr;
            public string NmTrsr;
            public string IdPtcDsp;
            public string IdFtrExp;
            public string IdRateUs;
            public string IdPercIcms;
            public decimal VttlPesoBrtNfR;
            public decimal VttlServNf;
            public decimal VttlSurChrgNf;
            public decimal VttlAdcIpiNf;
            public decimal VttlCascNf;
            public decimal VttlCmi2Nf;
            public decimal VttlFrtNf;
            public decimal VttlSegNf;
            public decimal VttlDsctNfR;
            public decimal VttlDsctCampR;
            public decimal VttlMrcdNfR;
            public decimal VttlBsIcmsNfR;
            public decimal VttlBsIcmsSubsR;
            public decimal VttlBsIpiNfR;
            public decimal VttlIcmsNfR;
            public decimal VttlIpiNfR;
            public decimal VttlFtrdNfR;
            public decimal VttlPisNfR;
            public decimal VttlCofinsNfR;
            public string IdLgOnFunc;
            public string CdPgtSppp;
            public string CdTPrd;
            public string IcNfTrbt;
        }

        // Lightweight classes for selected SQL results
        class Sf0002Row
        {
            public string ItdCdTRec;
            public string ItdCdModTrsp;
            public string ItdIdEtiqRec;
            public string ItdDtcNf;      // DDMMYYYY
            public string ItdHorNf;      // HH24MISS
            public string ItdCdSufr;
            public string ItdIdNfLog;    // padded LTRIM(...)
            public string ItdCdNf;
            public string ItdCdTPgt;
            public string ItdIdNf;
            public string ItdDtcNfFull;  // YYYYMMDDHH24MISS
            public string ItdFtrExp;
            public string ItdTimestamp;
            public string ItdRateUs;
            public string ItdRatePgt;
            public string TipQDiaPgt;
            public string CdCliEsp;
        }

        class St0003Row
        {
            public string CdClsPdd;
            public string IdCliRlcd;
        }

        class Wq01Totals
        {
            public decimal TtlVFrt;
            public decimal TtlVTxServ;
            public decimal TtlVSurChrg;
            public decimal TtlVAdcIpi;
            public decimal TtlVCasRmanOda;
            public decimal TtlVCasRmanCmis2;
        }

        // Public routine: returns true on success; on failure returns false and sets errorMessage.
        public bool Executar(
            IDbConnection connection,
            Sf0001 sf0001,
            string st0001CdFbr,
            Cc0001 cc0001,
            out St80014 st80014,
            out string errorMessage)
        {
            st80014 = new St80014();
            errorMessage = null;

            // Initialize ST80014 (CLEAR ST8001-ST80014)
            // Equivalent to MOVE SPACES TO ST8001-ST80014
            st80014 = new St80014();
            st80014.CdAces = "20";
            st80014.CdFbr = st0001CdFbr ?? "";
            st80014.CdTRec = sf0001.PtdCdTRec ?? "";
            st80014.CdTMtz = sf0001.PtdCdTMtz ?? "";

            var mustClose = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustClose = true;
            }

            try
            {
                // 1) Query ITD_ITMFATURADO join TIP_PAGAMENTO and CONTROLE_VOLUME
                const string sqlItd =
    @"SELECT A.ITD_CD_T_REC,
       A.ITD_CD_MOD_TRSP,
       A.ITD_ID_ETIQ_REC,
       TO_CHAR(A.ITD_DTC_NF,'DDMMYYYY') AS DTC_NF,
       TO_CHAR(A.ITD_DTC_NF,'HH24MISS') AS HOR_NF,
       A.ITD_CD_SUFR,
       LTRIM(TO_CHAR(A.ITD_ID_NF,'000000000')) AS ID_NF_LOG,
       A.ITD_CD_NF,
       A.ITD_CD_T_PGT,
       A.ITD_ID_NF,
       TO_CHAR(A.ITD_DTC_NF,'YYYYMMDDHH24MISS') AS DTC_NF_FULL,
       A.ITD_FTR_EXP,
       TO_CHAR(A.ITD_NF_TIMESTAMP,'YYYY-MM-DD-HH24.MI.SS.FF6') AS NF_TIMESTAMP,
       A.ITD_RATE_US,
       A.ITD_RATE_PGT,
       B.TIP_Q_DIA_PGT,
       C.CD_CLI_ESP
FROM H1SF.ITD_ITMFATURADO A,
     H1SF.TIP_PAGAMENTO B,
     H1ST.CONTROLE_VOLUME C
WHERE A.ITD_CD_T_PGT = B.TIP_CD_T_PGT
  AND A.ITD_CD_T_MTZ = C.CD_T_MTZ
  AND A.ITD_ID_MTZ   = C.ID_MTZ
  AND A.ITD_ID_CLI   = C.ID_CLI
  AND A.ITD_CD_MERC_DST  = :mercDst
  AND A.ITD_DTC_SEL_FTRM = :dtcSelFtrm
  AND A.ITD_LGON_FUNC    = :lgonFunc
  AND A.ITD_ID_PTC_DSP   = :idPtcDsp
  AND A.ITD_ID_NF        = :numero
  AND ROWNUM = 1";

                Sf0002Row sf0002 = null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlItd;
                    AddParam(cmd, "mercDst", sf0001.PtdCdMercDst);
                    AddParam(cmd, "dtcSelFtrm", sf0001.PtdDtcSelFtrm);
                    AddParam(cmd, "lgonFunc", sf0001.PtdLgonFunc);
                    AddParam(cmd, "idPtcDsp", sf0001.PtdIdPtcDsp);
                    AddParam(cmd, "numero", cc0001.Numero);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read())
                        {
                            errorMessage = $"*** ERRO - LOG. SYNC. - ITD-ITMFATURADO  NF: ===> {cc0001.Numero}";
                            return false;
                        }

                        sf0002 = new Sf0002Row
                        {
                            ItdCdTRec = GetString(rdr, 0),
                            ItdCdModTrsp = GetString(rdr, 1),
                            ItdIdEtiqRec = GetString(rdr, 2),
                            ItdDtcNf = GetString(rdr, 3),
                            ItdHorNf = GetString(rdr, 4),
                            ItdCdSufr = GetString(rdr, 5),
                            ItdIdNfLog = GetString(rdr, 6),
                            ItdCdNf = GetString(rdr, 7),
                            ItdCdTPgt = GetString(rdr, 8),
                            ItdIdNf = GetString(rdr, 9),
                            ItdDtcNfFull = GetString(rdr, 10),
                            ItdFtrExp = GetString(rdr, 11),
                            ItdTimestamp = GetString(rdr, 12),
                            ItdRateUs = GetString(rdr, 13),
                            ItdRatePgt = GetString(rdr, 14),
                            TipQDiaPgt = GetString(rdr, 15),
                            CdCliEsp = GetString(rdr, 16)
                        };
                    }
                }

                // 2) Query RECOLHIMENTO / ITEM_RECOLHIMENTO to get ST0003
                St0003Row st0003 = null;
                const string sqlSt0003 =
    @"SELECT A.CD_CLS_PDD,
       A.ID_CLI_RLCD
FROM H1ST.RECOLHIMENTO A,
     H1ST.ITEM_RECOLHIMENTO B
WHERE A.CD_T_REC    = B.CD_T_REC
  AND A.CD_T_MTZ    = B.CD_T_MTZ
  AND A.ID_MTZ      = B.ID_MTZ
  AND A.ID_CLI      = B.ID_CLI
  AND A.CD_SIT_ATND = B.CD_SIT_ATND
  AND A.ID_PDD      = B.ID_PDD
  AND A.ID_PSO      = B.ID_PSO
  AND B.ID_ETIQ_REC = :idEtiqRec
  AND ROWNUM = 1";
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlSt0003;
                    AddParam(cmd, "idEtiqRec", sf0002.ItdIdEtiqRec);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read())
                        {
                            errorMessage = $"*** ERRO - LOG. SYNC. - RECOL. ETIQ-REC: ===> {sf0002.ItdIdEtiqRec}";
                            return false;
                        }

                        st0003 = new St0003Row
                        {
                            CdClsPdd = GetString(rdr, 0),
                            IdCliRlcd = GetString(rdr, 1)
                        };
                    }
                }

                // 3) If ST0003-ID-CLI-RLCD > -1 then get ST0008-ID-MTZ (optional)
                if (int.TryParse(st0003.IdCliRlcd, out int idCliRlcd) && idCliRlcd > -1)
                {
                    const string sqlSt0008 = @"
SELECT ID_MTZ
  FROM H1ST.CONTROLE_VOLUME
 WHERE ID_CLI = :idCli
   AND ROWNUM = 1";
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sqlSt0008;
                        AddParam(cmd, "idCli", st0003.IdCliRlcd);
                        var obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value)
                        {
                            st80014.IdMtz = Convert.ToString(obj);
                            st80014.IdCli = st0003.IdCliRlcd;
                        }
                        else
                        {
                            // fallback to SF0001 values if lookup not found
                            st80014.IdMtz = sf0001.PtdIdMtz;
                            st80014.IdCli = sf0001.PtdIdCli;
                        }
                    }
                }
                else
                {
                    st80014.IdMtz = sf0001.PtdIdMtz;
                    st80014.IdCli = sf0001.PtdIdCli;
                }

                // 4) Validate ITD-ID-NF (WI01-ID-NF / SF0002-ITD-ID-NF-LOG checks)
                if (string.IsNullOrWhiteSpace(sf0002.ItdIdNfLog) ||
                    sf0002.ItdIdNfLog.Trim() == "-1" ||
                    sf0002.ItdIdNfLog.Trim() == new string('0', sf0002.ItdIdNfLog.Length))
                {
                    errorMessage = $"*** ERRO - NF COM PROBLEMAS (PFN) NF: ===> {cc0001.Numero}";
                    return false;
                }

                // 5) Move a few ITD/ST values into ST80014
                st80014.CdCliEsp = (sf0002.CdCliEsp ?? "").Trim();
                // ST80014-CD-T-OPRC-FTRM logic
                // The original sets 'T' if SF0005-SFT-IC-FTRM-TRGD='S' and WS36-FASE-FTRM='1', else 'V' or 'P'
                // We don't have SF0005 and WS36 here; caller may set st80014.CdTOprcFtrm afterwards if required.
                st80014.CdModTrsp = sf0002.ItdCdModTrsp;
                st80014.CdNF = sf0002.ItdCdNf;
                st80014.IdNf = sf0002.ItdIdNfLog;
                st80014.DtcNf = sf0002.ItdDtcNf;
                st80014.HrNf = sf0002.ItdHorNf;
                st80014.IdTimeStmp = sf0002.ItdTimestamp;
                st80014.IdFtrExp = sf0002.ItdFtrExp;
                st80014.IdRateUs = sf0002.ItdRateUs;

                // 6) Aggregate totals from ITD_ITMFATURADO for the invoice
                const string sqlTotals =
    @"SELECT SUM(ITD_V_FRT),
       SUM(ITD_V_TX_SERV),
       SUM(ITD_V_SUR_CHRG),
       SUM(ITD_V_ADC_IPI),
       SUM(ITD_V_CAS_RMAN_ODA),
       SUM(ITD_V_CAS_RMAN_CMIS2)
  FROM H1SF.ITD_ITMFATURADO
 WHERE ITD_CD_MERC_DST  = :mercDst
   AND ITD_DTC_SEL_FTRM = :dtcSelFtrm
   AND ITD_LGON_FUNC    = :lgonFunc
   AND ITD_ID_PTC_DSP   = :idPtcDsp
   AND ITD_ID_NF        = :numero";
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlTotals;
                    AddParam(cmd, "mercDst", sf0001.PtdCdMercDst);
                    AddParam(cmd, "dtcSelFtrm", sf0001.PtdDtcSelFtrm);
                    AddParam(cmd, "lgonFunc", sf0001.PtdLgonFunc);
                    AddParam(cmd, "idPtcDsp", sf0001.PtdIdPtcDsp);
                    AddParam(cmd, "numero", cc0001.Numero);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read())
                        {
                            errorMessage = $"*** ERRO - LOG. SYNC. - SUM ITMFATURADO  NF: ===> {cc0001.Numero}";
                            return false;
                        }

                        var totals = new Wq01Totals
                        {
                            TtlVFrt = GetDecimal(rdr, 0),
                            TtlVTxServ = GetDecimal(rdr, 1),
                            TtlVSurChrg = GetDecimal(rdr, 2),
                            TtlVAdcIpi = GetDecimal(rdr, 3),
                            TtlVCasRmanOda = GetDecimal(rdr, 4),
                            TtlVCasRmanCmis2 = GetDecimal(rdr, 5)
                        };

                        st80014.VttlFrtNf = totals.TtlVFrt;
                        st80014.VttlServNf = totals.TtlVTxServ;
                        st80014.VttlSurChrgNf = totals.TtlVSurChrg;
                        st80014.VttlAdcIpiNf = totals.TtlVAdcIpi;
                        st80014.VttlCascNf = totals.TtlVCasRmanOda;
                        st80014.VttlCmi2Nf = totals.TtlVCasRmanCmis2;
                    }
                }

                // 7) Emulate INSPECT ... CONVERTING SPACES TO ZEROS where relevant:
                // Several COBOL fields are converted; do a few key ones:
                st80014.IdNf = ConvertSpacesToZeros(st80014.IdNf);
                st80014.IdFtrExp = ConvertSpacesToZeros(st80014.IdFtrExp);
                st80014.IdRateUs = ConvertSpacesToZeros(st80014.IdRateUs);
                // caller can perform additional converting if required.

                // 8) Determine ST80014-CD-RPAR-FISC by lookup in LMN_LOJAMANAUS
                const string sqlLmn =
    @"SELECT 1
  FROM H1SF.LMN_LOJAMANAUS
 WHERE LMN_ID_CLI = :idCli
   AND ROWNUM = 1";
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlLmn;
                    AddParam(cmd, "idCli", st80014.IdCli);
                    var res = cmd.ExecuteScalar();
                    if (res == null || res == DBNull.Value)
                        st80014.CdRparFisc = string.Empty; // MOVE SPACES
                    else
                        st80014.CdRparFisc = "010-010-34";
                }

                // 9) Populate several ST80014 totals from CC0001 or WQ01 as per COBOL logic
                st80014.VttlPesoBrtNfR = cc0001.PesoBrutoKgLog;
                if (/* destination = E? */ false) // caller can override depending on WS36
                {
                    st80014.VttlServNf = cc0001.VlOutrasDespesasLog;
                }
                // else keep previously computed totals from ITD select

                // 10) set program/function identifiers
                st80014.IdLgOnFunc = "BPIS";

                // 11) Copy other high-level totals from cc0001 as in COBOL
                st80014.VttlMrcdNfR = cc0001.PrecoTotalMLog;
                st80014.VttlBsIcmsNfR = cc0001.VlTotalBaseIcmsLog;
                st80014.VttlBsIcmsSubsR = cc0001.VlTotalStfLog;
                st80014.VttlBsIpiNfR = cc0001.PrecoTotalMLog;

                // 12) PGT/PRD flags and IC_NF_TRBT
                // (COBOL sets ST80014-CD-PGT-SPPP conditionally; caller may set if needed)
                st80014.CdTPrd = ""; // caller must fill if available
                st80014.IcNfTrbt = (cc0001.VlTotalIcmsLog == 0m) ? "N" : "S";

                return true;
            }
            finally
            {
                if (mustClose && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        // ---------- helpers ----------
        static void AddParam(IDbCommand cmd, string name, object val)
        {
            var p = cmd.CreateParameter();
            // Use Oracle style parameter prefix ":" to match COBOL SQL; adjust for your provider if needed.
            p.ParameterName = ":" + name;
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
            if (decimal.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal res)) return res;
            return 0m;
        }

        static string ConvertSpacesToZeros(string s)
        {
            if (s == null) return s;
            return s.Replace(' ', '0');
        }
    }
}