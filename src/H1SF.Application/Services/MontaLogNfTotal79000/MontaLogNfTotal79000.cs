using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    public class MontaLogNfTotal79000 : IMontaLogNfTotal79000
    {
        /// <summary>
        /// Converted from COBOL: 790-00-MONTA-LOG-NF-TOTAL
        /// Performs the MOVE/INSPECT semantics and populates the MQ message fields.
        /// </summary>
        public void Executar(
            ref string ws35AuxTs,
            ref string acb5022CdAces,
            ref int acb5022CdRetrEci,
            ref int acb5022CdRetrAces,
            ref string acb50221CdAcesMq,
            ref int acb50221IdFilaMq,
            ref int acb50221CdRetrMq,
            ref int acb50221CdComp,
            ref int acb50221CdRson,
            ref string acb50221IdMsgPrc,
            string sf0002ItdDtcSelFtrm,
            string cc0001NumeroCtb5,
            ref string ws35IdCorrIdLitSc,
            ref string ws35IdCorrIdAlfSc,
            ref string ws35IdCorrelId,
            ref string acb50221IdMsgAux,
            object mq01AreCntFila,
            ref object acb50221AreCntFila
        )
        {
            // LOG token
            ws35AuxTs = "790-00-MONTA-LOG-NF-TOTAL";

            // ACB5022 initialisation
            acb5022CdAces = "01";
            acb5022CdRetrEci = 0;
            acb5022CdRetrAces = 0;

            // ACB50221 initialisation
            acb50221CdAcesMq = "03";
            acb50221IdFilaMq = 5; // COBOL literal 00005
            acb50221CdRetrMq = 0;
            acb50221CdComp = 0;
            acb50221CdRson = 0;
            acb50221IdMsgPrc = string.Empty;

            // Build correlation id parts
            ws35IdCorrIdLitSc = sf0002ItdDtcSelFtrm ?? string.Empty;
            ws35IdCorrIdAlfSc = cc0001NumeroCtb5 ?? string.Empty;

            // WS35-ID-CORREL-ID is 24 chars in COBOL: concatenate, pad/truncate to 24, then convert spaces to '0'
            string combined = (ws35IdCorrIdLitSc + ws35IdCorrIdAlfSc);
            if (combined.Length < 24)
                combined = combined.PadRight(24, ' ');
            else if (combined.Length > 24)
                combined = combined.Substring(0, 24);

            ws35IdCorrelId = combined.Replace(' ', '0');

            // Move WS35-ID-CORREL-ID to ACB50221-ID-MSG-AUX
            acb50221IdMsgAux = ws35IdCorrelId;

            // Move MQ area counter
            acb50221AreCntFila = mq01AreCntFila;
        }
    }
}