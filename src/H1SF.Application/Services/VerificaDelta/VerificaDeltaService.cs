using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class VerificaDeltaService : IVerificaDeltaService
    {
        private readonly IDbConnection _connection;

        public VerificaDeltaService(IDbConnection connection)
        {
            _connection = connection;
        }

        public string VerificaDelta(
            string mercadoria,
            DateTime dataSelecao,
            string loginFunc,
            string idProcesso,
            string numeroNF)
        {
            // MOVE '600-00-VERIFICA-DELTA' TO WS35-AUX-TS
            string rotina = "600-00-VERIFICA-DELTA";

            // MOVE '5' TO WS01-CD-T-DSCT-CAMP-AUX
            string codigoDescontoAux = "5";

            string sql = @"
            SELECT A.CD_T_PGM
            FROM   H1ST.ITEM_RECOLHIMENTO A
            JOIN   H1SF.ITD_ITMFATURADO B
                   ON A.ID_ETIQ_REC = B.ITD_ID_ETIQ_REC
            WHERE  B.ITD_CD_MERC_DST  = :CD_MERC
              AND  B.ITD_DTC_SEL_FTRM = :DTC_SEL
              AND  B.ITD_LGON_FUNC    = :LGON_FUNC
              AND  B.ITD_ID_PTC_DSP   = :PTC_DSP
              AND  B.ITD_ID_NF        = :NUM_NF
        ";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = sql;

                AddParameter(command, "CD_MERC", mercadoria);
                AddParameter(command, "DTC_SEL", dataSelecao);
                AddParameter(command, "LGON_FUNC", loginFunc);
                AddParameter(command, "PTC_DSP", idProcesso);
                AddParameter(command, "NUM_NF", numeroNF);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cdTPgm = reader["CD_T_PGM"]?.ToString()?.Trim();

                        // IF WQ01-CD-T-PGM GREATER SPACES
                        if (!string.IsNullOrWhiteSpace(cdTPgm))
                        {
                            // IF WQ01-CD-T-PGM = 'DELT'
                            if (cdTPgm == "DELT")
                            {
                                // MOVE '4'
                                codigoDescontoAux = "4";
                            }
                            else
                            {
                                // MOVE '1'
                                codigoDescontoAux = "1";
                            }

                            break; // GO TO 600-20
                        }

                        // GO TO 600-10 = while loop (já atendido naturalmente)
                    }
                }
            }

            // 600-20 CLOSE / EXIT
            return codigoDescontoAux;
        }

        private void AddParameter(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }

}
