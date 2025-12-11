using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{
    /// <summary>
    /// Converted from COBOL: 791-00-MONTA-LOG-NF-ITEM
    /// Mutates the provided fields the same way COBOL MOVE/INSPECT do.
    /// </summary>

    public class MontaLogNfItem : IMontaLogNfItem
    {
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
    ref string ws35IdCorrIdLitSc,    // WS35-ID-CORR-ID-LIT-SC
    ref string ws35IdCorrIdAlfSc,    // WS35-ID-CORR-ID-ALF-SC
    ref string ws35IdCorrelId,       // WS35-ID-CORREL-ID (24 chars)
    ref string acb50221IdMsgAux,     // ACB50221-ID-MSG-AUX
    object mq01AreCntFila,
    ref object acb50221AreCntFila    // ACB50221-ARE-CNT-FILA
)
        {
            // LOG token
            ws35AuxTs = "791-00-MONTA-LOG-NF-ITEM";

            // initialize ACB5022
            acb5022CdAces = "01";
            acb5022CdRetrEci = 0;
            acb5022CdRetrAces = 0;

            // initialize ACB50221
            acb50221CdAcesMq = "03";
            acb50221IdFilaMq = 5; // COBOL literal 00005
            acb50221CdRetrMq = 0;
            acb50221CdComp = 0;
            acb50221CdRson = 0;
            acb50221IdMsgPrc = string.Empty;

            // build correlation id parts (COBOL MOVE)
            ws35IdCorrIdLitSc = sf0002ItdDtcSelFtrm ?? string.Empty;
            ws35IdCorrIdAlfSc = cc0001NumeroCtb5 ?? string.Empty;

            // WS35-ID-CORREL-ID is 24 chars in COBOL: concatenate then convert spaces to zeros.
            // emulate INSPECT ... CONVERTING SPACES TO ZEROS
            string combined = (ws35IdCorrIdLitSc + ws35IdCorrIdAlfSc);
            // Ensure at least 24 characters (COBOL field length); pad with spaces if shorter,
            // then convert spaces to '0' to match INSPECT CONVERTING SPACES TO ZEROS.
            if (combined.Length < 24)
                combined = combined.PadRight(24, ' ');
            else if (combined.Length > 24)
                combined = combined.Substring(0, 24);

            ws35IdCorrelId = combined.Replace(' ', '0');

            // MOVE WS35-ID-CORREL-ID TO ACB50221-ID-MSG-AUX
            acb50221IdMsgAux = ws35IdCorrelId;

            // MOVE MQ01-ARE-CNT-FILA TO ACB50221-ARE-CNT-FILA
            acb50221AreCntFila = mq01AreCntFila;
        }
    }
}