using System;
using System.Data;
using System.Globalization;

public class AtualizaOdaCmis2 : IAtualizaOdaCmis2
{
    // Converted from COBOL: 567-00-ATUALIZA-ODA-CMIS2
    public void Executar(
        IDbConnection connection,
        ref string ws35AuxTs,
        string cb0004CdFbr,
        string sf0001PtdCdMercDst,
        string sf0001PtdDtcSelFtrm,
        string sf0001PtdLgonFunc,
        string sf0001PtdIdPtcDsp,
        string cc0001Numero,
        string cc0002IdfNum,
        decimal cc0002VlAliqPis5,
        decimal cc0002VlAliqCofins5)
    {
        ws35AuxTs = "567-00-ATUALIZA-ODA-CMIS2";

        var ensureOpen = connection.State != ConnectionState.Open;
        if (ensureOpen) connection.Open();

        try
        {
            // 1) Read B.CD_T_PREC_FTRM into st0005CdTPrecFtrm
            string st0005CdTPrecFtrm = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText =
@"SELECT B.CD_T_PREC_FTRM
  FROM H1SF.ITD_ITMFATURADO A, H1ST.ITEM_RECOLHIMENTO B
 WHERE A.ITD_ID_ETIQ_REC  = B.ID_ETIQ_REC
   AND A.ITD_CD_MERC_DST  = @mercDst
   AND A.ITD_DTC_SEL_FTRM = @dtcSelFtrm
   AND A.ITD_LGON_FUNC    = @lgonFunc
   AND A.ITD_ID_PTC_DSP   = @idPtcDsp
   AND A.ITD_ID_NF        = @numero
   AND A.ITD_CD_SEQ_ITM   = @seqItm
   AND ROWNUM = 1";
                AddParameter(cmd, "@mercDst", sf0001PtdCdMercDst);
                AddParameter(cmd, "@dtcSelFtrm", sf0001PtdDtcSelFtrm);
                AddParameter(cmd, "@lgonFunc", sf0001PtdLgonFunc);
                AddParameter(cmd, "@idPtcDsp", sf0001PtdIdPtcDsp);
                AddParameter(cmd, "@numero", cc0001Numero);
                AddParameter(cmd, "@seqItm", cc0002IdfNum);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                        st0005CdTPrecFtrm = rdr.GetString(0);
                }
            }

            // 2) COBOL: if CB0004-CD-FBR = '28' AND ST0005-CD-T-PREC-FTRM = 'T' => exit
            if (string.Equals(cb0004CdFbr, "28", StringComparison.Ordinal) &&
                string.Equals(st0005CdTPrecFtrm, "T", StringComparison.Ordinal))
            {
                return;
            }

            // 3) Read the ITD row values needed to compute the new ITD_V_CAS_RMAN_CMIS2
            decimal? itdVcasRmanCmis2 = null;
            decimal? itdQQtdPecaFtrd = null;
            decimal? itdVcasRmanOda = null;

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText =
@"SELECT ITD_V_CAS_RMAN_CMIS2, ITD_Q_PECA_FTRD, ITD_V_CAS_RMAN_ODA
  FROM H1SF.ITD_ITMFATURADO A
 WHERE A.ITD_CD_MERC_DST  = @mercDst
   AND A.ITD_DTC_SEL_FTRM = @dtcSelFtrm
   AND A.ITD_LGON_FUNC    = @lgonFunc
   AND A.ITD_ID_PTC_DSP   = @idPtcDsp
   AND A.ITD_ID_NF        = @numero
   AND A.ITD_CD_SEQ_ITM   = @seqItm
   AND ROWNUM = 1";
                AddParameter(cmd, "@mercDst", sf0001PtdCdMercDst);
                AddParameter(cmd, "@dtcSelFtrm", sf0001PtdDtcSelFtrm);
                AddParameter(cmd, "@lgonFunc", sf0001PtdLgonFunc);
                AddParameter(cmd, "@idPtcDsp", sf0001PtdIdPtcDsp);
                AddParameter(cmd, "@numero", cc0001Numero);
                AddParameter(cmd, "@seqItm", cc0002IdfNum);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0)) itdVcasRmanCmis2 = Convert.ToDecimal(rdr.GetValue(0), CultureInfo.InvariantCulture);
                        if (!rdr.IsDBNull(1)) itdQQtdPecaFtrd = Convert.ToDecimal(rdr.GetValue(1), CultureInfo.InvariantCulture);
                        if (!rdr.IsDBNull(2)) itdVcasRmanOda = Convert.ToDecimal(rdr.GetValue(2), CultureInfo.InvariantCulture);
                    }
                }
            }

            // If there's nothing to update (no row or NVL(...,0) <= 0) then exit.
            if (!itdVcasRmanCmis2.HasValue || itdVcasRmanCmis2.GetValueOrDefault() <= 0m)
                return;

            if (!itdQQtdPecaFtrd.HasValue || itdQQtdPecaFtrd.GetValueOrDefault() == 0m)
                return; // avoid division by zero

            // 4) Compute new value following COBOL/Oracle expression:
            // ((ITD_V_CAS_RMAN_CMIS2/ITD_Q_PECA_FTRD) -
            //   TRUNC(ROUND((ITD_V_CAS_RMAN_ODA/ITD_Q_PECA_FTRD) * (pis+cof) / 10000,2))
            // ) * ITD_Q_PECA_FTRD
            decimal q = itdQQtdPecaFtrd.Value;
            decimal current = itdVcasRmanCmis2.Value;
            decimal oda = itdVcasRmanOda.GetValueOrDefault(0m);
            decimal rateSum = cc0002VlAliqPis5 + cc0002VlAliqCofins5;

            // (ITD_V_CAS_RMAN_ODA / Q) * (pis + cof) / 10000  -> round to 2 decimals
            decimal raw = (oda / q) * rateSum / 10000m;
            decimal rounded2 = Math.Round(raw, 2, MidpointRounding.AwayFromZero);
            decimal truncated = Math.Truncate(rounded2);

            decimal newValue = ((current / q) - truncated) * q;

            // 5) Perform the UPDATE using computed newValue
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText =
@"UPDATE H1SF.ITD_ITMFATURADO
   SET ITD_V_CAS_RMAN_CMIS2 = @newValue
 WHERE ITD_CD_MERC_DST  = @mercDst
   AND ITD_DTC_SEL_FTRM = @dtcSelFtrm
   AND ITD_LGON_FUNC    = @lgonFunc
   AND ITD_ID_PTC_DSP   = @idPtcDsp
   AND ITD_ID_NF        = @numero
   AND ITD_CD_SEQ_ITM   = @seqItm
   AND NVL(ITD_V_CAS_RMAN_CMIS2,0) > 0";
                AddParameter(cmd, "@newValue", newValue);
                AddParameter(cmd, "@mercDst", sf0001PtdCdMercDst);
                AddParameter(cmd, "@dtcSelFtrm", sf0001PtdDtcSelFtrm);
                AddParameter(cmd, "@lgonFunc", sf0001PtdLgonFunc);
                AddParameter(cmd, "@idPtcDsp", sf0001PtdIdPtcDsp);
                AddParameter(cmd, "@numero", cc0001Numero);
                AddParameter(cmd, "@seqItm", cc0002IdfNum);

                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (ensureOpen && connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    static void AddParameter(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value ?? (object)DBNull.Value;
        cmd.Parameters.Add(p);
    }
}