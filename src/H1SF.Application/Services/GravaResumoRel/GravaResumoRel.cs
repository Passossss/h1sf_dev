using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class GravaResumoRel : IGravaResumoRel
    {
        // Converted from COBOL: 550-00-GRAVA-RESUMO-REL
        // Parameters map the COBOL working-storage fields used by the routine.
        public void Executar(
            IDbConnection connection,
            string st0001CdRegrFtrm,
            string cb0004CdTPrd,
            string cb0001RreIdPrcpPtdLit,
            ref string ws31ChvComandoPjl,          // mutated (RESET-CHV sets to 'N')
            ref string cb0001RreCdSqnPjl,          // mutated by selection
            string cb0001RreCdStm,
            string cb0001RreDtcGrc,                // expected format "yyyyMMddHHmmss"
            string cb0001RreIdPrcp,
            string cb0001RreCdSqnDct,
            string cb0001RreIdAuxImps1,
            string cb0001RreIdAuxImps2,
            string cb0001RreIdAuxImps3,
            string cb0001RreIdAuxImps4,
            string cb0001RreIdAuxImps5,
            string wi01RreIdAuxImps5,
            string cb0001RreIdRel,
            string cb0001RreIdImpr)
        {
            // Translate the COBOL conditional that jumps to RESET-CHV
            if (((st0001CdRegrFtrm == "M" || st0001CdRegrFtrm == "K" || cb0004CdTPrd == "C")
                 && cb0001RreIdPrcpPtdLit == "PROTOCOL")
                || (st0001CdRegrFtrm == "N" && cb0001RreIdRel == "DANFE DE PECAS"))
            {
                // COBOL: GO TO 550-10-RESET-CHV  -> set WS31-CHV-COMANDO-PJL = 'N' and exit
                ws31ChvComandoPjl = "N";
                return;
            }

            // If PJL command flag is 'N', select sequence code
            if (ws31ChvComandoPjl == "N")
            {
                switch (cb0001RreIdPrcpPtdLit)
                {
                    case "PROTOCOL":
                        cb0001RreCdSqnPjl = "01";
                        break;
                    case "INSTRUCA":
                        cb0001RreCdSqnPjl = "02";
                        break;
                    case "PACKLIST":
                        cb0001RreCdSqnPjl = "05";
                        break;
                    case "LISTAEMB":
                        cb0001RreCdSqnPjl = "06";
                        break;
                    default:
                        cb0001RreCdSqnPjl = "03";
                        break;
                }
            }

            // Prepare the insert. COBOL used TO_DATE(:CB0001-RRE-DTC-GRC,'YYYYMMDDHH24MISS')
            // We parse the incoming string to DateTime and insert as proper DB parameter.
            DateTime? dtcGrc = null;
            if (!string.IsNullOrWhiteSpace(cb0001RreDtcGrc))
            {
                if (DateTime.TryParseExact(cb0001RreDtcGrc, "yyyyMMddHHmmss",
                                           System.Globalization.CultureInfo.InvariantCulture,
                                           System.Globalization.DateTimeStyles.None,
                                           out DateTime parsed))
                {
                    dtcGrc = parsed;
                }
                else
                {
                    // If parse fails, leave null and let DB handle or throw as appropriate
                    dtcGrc = null;
                }
            }

            // choose value for AUX_IMPS_5 (COBOL shows two sources in VALUES list)
            var auxImps5 = !string.IsNullOrWhiteSpace(cb0001RreIdAuxImps5)
                ? cb0001RreIdAuxImps5
                : wi01RreIdAuxImps5;

            // Build parameterized SQL (use @-parameters; provider will map names as needed).
            const string sql = @"
INSERT INTO H1CB.RRE_RESREL
  (RRE_CD_STM, RRE_DTC_GRC, RRE_ID_PRCP, RRE_CD_SQN_DCT, RRE_CD_SQN_PJL,
   RRE_ID_AUX_IMPS_1, RRE_ID_AUX_IMPS_2, RRE_ID_AUX_IMPS_3, RRE_ID_AUX_IMPS_4, RRE_ID_AUX_IMPS_5,
   RRE_ID_REL, RRE_ID_IMPR)
VALUES
  (@cdStm, @dtcGrc, @idPrcp, @cdSqnDct, @cdSqnPjl,
   @aux1, @aux2, @aux3, @aux4, @aux5,
   @idRel, @idImpr)";

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                // Add parameters in a provider-agnostic way
                var addParam = new Action<string, object>((name, value) =>
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = name;
                    p.Value = value ?? (object)DBNull.Value;
                    cmd.Parameters.Add(p);
                });

                addParam("@cdStm", string.IsNullOrEmpty(cb0001RreCdStm) ? (object)DBNull.Value : cb0001RreCdStm);
                addParam("@dtcGrc", dtcGrc.HasValue ? (object)dtcGrc.Value : DBNull.Value);
                addParam("@idPrcp", string.IsNullOrEmpty(cb0001RreIdPrcp) ? (object)DBNull.Value : cb0001RreIdPrcp);
                addParam("@cdSqnDct", string.IsNullOrEmpty(cb0001RreCdSqnDct) ? (object)DBNull.Value : cb0001RreCdSqnDct);
                addParam("@cdSqnPjl", string.IsNullOrEmpty(cb0001RreCdSqnPjl) ? (object)DBNull.Value : cb0001RreCdSqnPjl);
                addParam("@aux1", string.IsNullOrEmpty(cb0001RreIdAuxImps1) ? (object)DBNull.Value : cb0001RreIdAuxImps1);
                addParam("@aux2", string.IsNullOrEmpty(cb0001RreIdAuxImps2) ? (object)DBNull.Value : cb0001RreIdAuxImps2);
                addParam("@aux3", string.IsNullOrEmpty(cb0001RreIdAuxImps3) ? (object)DBNull.Value : cb0001RreIdAuxImps3);
                addParam("@aux4", string.IsNullOrEmpty(cb0001RreIdAuxImps4) ? (object)DBNull.Value : cb0001RreIdAuxImps4);
                addParam("@aux5", string.IsNullOrEmpty(auxImps5) ? (object)DBNull.Value : auxImps5);
                addParam("@idRel", string.IsNullOrEmpty(cb0001RreIdRel) ? (object)DBNull.Value : cb0001RreIdRel);
                addParam("@idImpr", string.IsNullOrEmpty(cb0001RreIdImpr) ? (object)DBNull.Value : cb0001RreIdImpr);

                // Ensure connection is open
                var shouldClose = false;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    shouldClose = true;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (shouldClose)
                        connection.Close();
                }
            }

            // COBOL falls through to 550-99-SQL-GRAVA-EXIT -> EXIT (method end)
        }
    }
}
