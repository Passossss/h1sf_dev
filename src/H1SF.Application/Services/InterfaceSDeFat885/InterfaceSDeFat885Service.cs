using System;
using System.Collections.Generic;

namespace H1SF.Application.Services.InterfaceSDeFat
{
    /// <summary>
    /// Tradução isolada do trecho COBOL 885-00-INTERFACE-S-DE-FAT para C#.
    /// Arquivo independente; consultas/formatações e gravação de interface são métodos protegidos virtuals para override/injeção.
    /// </summary>
    public class InterfaceSDeFat885Service : IInterfaceSDeFat885Service
    {
        // Entradas / contexto
        public string ST0001CdRegrFtrm { get; set; } = string.Empty;
        public string SF0001PtdDtcSelFtrm { get; set; } = string.Empty; // 'YYYYMMDDHH24MISS'
        public string SF0002ItdFtrExp { get; set; } = string.Empty;
        public string SF0001PtdIdCli { get; set; } = string.Empty;
        public string SF0001PtdCdTRec { get; set; } = string.Empty;
        public string SF0002ItdCdModTrspLog { get; set; } = string.Empty;
        public string SF0002ItdIdPddLog { get; set; } = string.Empty;
        public string SF0002ItdIdFtrApiLog { get; set; } = string.Empty;

        public int WS01QTtlItemFat { get; set; }

        // Campos destino / SE* structures (mantidos nomes COBOL-like)
        public string SE0001IdeIdTItf { get; set; } = string.Empty;
        public string SE0001IdeIdStmOrgm { get; set; } = string.Empty;
        public string SE0001IdeTxtMsg { get; set; } = string.Empty;
        public long SE0001IdeNumTReg { get; set; }

        public string SE90041IdFtrS57 { get; set; } = string.Empty;
        public string SE90041IdCli { get; set; } = string.Empty;
        public string SE90041DtcEms { get; set; } = string.Empty;
        public int SE90041QtItem { get; set; }
        public string SE90041CdTRec { get; set; } = string.Empty;
        public string SE90041CdModTrsp { get; set; } = string.Empty;
        public string SE90041IdPdd { get; set; } = string.Empty;
        public string SE90041IdFtrApi { get; set; } = string.Empty;
        public string SE90041TxtMsg { get; set; } = string.Empty;

        public string SE90241TxtMsg { get; set; } = string.Empty;

        // Campos auxiliares
        public string WQ01DtInlFtrm { get; set; } = string.Empty;

        public InterfaceSDeFat885Service()
        {
        }

        // Entrada principal que reúne as sections do COBOL 885
        public virtual void Execute()
        {
            IdentificaInterface();
            CarregaCampos();
            FinalizaInterface();
            CarregaCamposFim();
        }

        // 885-10-IDENTIFICA-INTERFACE
        protected virtual void IdentificaInterface()
        {
            if (string.Equals(ST0001CdRegrFtrm, "I", StringComparison.Ordinal))
            {
                SE0001IdeIdTItf = "FATURA_S57";
                SE0001IdeIdStmOrgm = "BPISPRA";
            }
            else if (string.Equals(ST0001CdRegrFtrm, "N", StringComparison.Ordinal))
            {
                SE0001IdeIdTItf = "FATURA_S7X";
                SE0001IdeIdStmOrgm = "BPISREB";
            }
            else
            {
                SE0001IdeIdTItf = "FATURA_S58";
                SE0001IdeIdStmOrgm = "BPIS";
            }
        }

        // 885-20-CARREGA-CAMPOS
        protected virtual void CarregaCampos()
        {
            // SELECT TO_CHAR(TO_DATE(:SF0001-PTD-DTC-SEL-FTRM,'YYYYMMDDHH24MISS'),'DD-MM-YYYY') INTO :WQ01-DT-INL-FTRM FROM DUAL
            WQ01DtInlFtrm = FormatDateToDdMmYyyy(SF0001PtdDtcSelFtrm);

            // MOVE SF0002-ITD-FTR-EXP TO SE90041-ID-FTR-S57.
            SE90041IdFtrS57 = ConvertSpacesToZeros(SF0002ItdFtrExp);

            // MOVE SF0001-PTD-ID-CLI TO SE90041-ID-CLI.
            SE90041IdCli = SF0001PtdIdCli;

            // MOVE WQ01-DT-INL-FTRM TO SE90041-DTC-EMS.
            SE90041DtcEms = WQ01DtInlFtrm;

            // MOVE WS01-Q-TTL-ITEM-FAT TO SE90041-QT-ITEM.
            SE90041QtItem = WS01QTtlItemFat;

            // Mapeamento de SF0001-PTD-CD-T-REC para SE90041-CD-T-REC (R1..R6)
            SE90041CdTRec = MapCdTRec(SF0001PtdCdTRec);

            // MOVE SF0002-ITD-CD-MOD-TRSP-LOG TO SE90041-CD-MOD-TRSP.
            SE90041CdModTrsp = SF0002ItdCdModTrspLog;

            // MOVE SF0002-ITD-ID-PDD-LOG TO SE90041-ID-PDD.
            SE90041IdPdd = SF0002ItdIdPddLog;

            // Condicional SF0002-ITD-ID-FTR-API-LOG or spaces
            if (string.Equals(ST0001CdRegrFtrm, "N", StringComparison.Ordinal))
                SE90041IdFtrApi = SF0002ItdIdFtrApiLog;
            else
                SE90041IdFtrApi = string.Empty;

            // Texto de mensagem: se registro tipo I usa SE90041-TXT-MSG, senão usa SE90241-TXT-MSG -> SE0001-IDE-TXT-MSG
            if (string.Equals(ST0001CdRegrFtrm, "I", StringComparison.Ordinal))
            {
                SE0001IdeTxtMsg = SE90041TxtMsg;
            }
            else
            {
                SE90241TxtMsg = SE90041TxtMsg; // COBOL movia SE90041->SE90241 then SE90241->SE0001
                SE0001IdeTxtMsg = SE90241TxtMsg;
            }

            // PERFORM 620-00-GRAVA-INTERFACE-DEA.
            GravaInterfaceDea();
        }

        // 885-30-FINALIZA-INTERFACE
        protected virtual void FinalizaInterface()
        {
            // Reaplica IDENTIFICA-INTERFACE logic
            IdentificaInterface();

            // MOVE 999999999 TO SE0001-IDE-NUM-T-REG
            SE0001IdeNumTReg = 999_999_999;
        }

        // 885-40-CARREGA-CAMPOS-FIM
        protected virtual void CarregaCamposFim()
        {
            // MOVE 'H1SF0033' TO WS01-FIM-DEA-PROCESSO.
            // Em COBOL esse campo era usado internamente; expor apenas através de gravação final.
            // PERFORM 620-00-GRAVA-INTERFACE-DEA.
            GravaInterfaceDea();
        }

        // -------------------------
        // Métodos substituíveis / stubs para integração e utilitários
        // -------------------------

        /// <summary>
        /// Formata data de 'YYYYMMDDHH24MISS' para 'DD-MM-YYYY'.
        /// Override se precisar de comportamento diferente (DB, cultura, validação).
        /// </summary>
        protected virtual string FormatDateToDdMmYyyy(string yyyymmddhh24miss)
        {
            if (string.IsNullOrWhiteSpace(yyyymmddhh24miss)) return string.Empty;

            // Tenta interpretar vários comprimentos (com ou sem hora)
            var formats = new[] { "yyyyMMddHHmmss", "yyyyMMddHHmm", "yyyyMMdd" };
            if (DateTime.TryParseExact(yyyymmddhh24miss, formats, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var dt))
            {
                return dt.ToString("dd-MM-yyyy");
            }

            // Fallback: tenta parse genérico
            if (DateTime.TryParse(yyyymmddhh24miss, out dt))
                return dt.ToString("dd-MM-yyyy");

            return string.Empty;
        }

        /// <summary>
        /// Converte espaços para '0' (INSPECT ... CONVERTING SPACES TO ZEROS).
        /// </summary>
        protected static string ConvertSpacesToZeros(string s)
        {
            if (s is null) return string.Empty;
            return s.Replace(' ', '0');
        }

        /// <summary>
        /// Mapeia código SF0001-PTD-CD-T-REC para 'R1'..'R6' conforme conjunto de valores COBOL.
        /// </summary>
        protected virtual string MapCdTRec(string cd)
        {
            if (string.IsNullOrEmpty(cd)) return string.Empty;
            cd = cd.ToUpperInvariant();

            var setR1 = new HashSet<string> { "R1", "RA", "RG", "U1", "UA", "UG" };
            var setR2 = new HashSet<string> { "R2", "RB", "RH", "U2", "UB", "UH" };
            var setR3 = new HashSet<string> { "R3", "RC", "RI", "U3", "UC", "UI" };
            var setR4 = new HashSet<string> { "R4", "RD", "RK", "U4", "UD", "UK" };
            var setR5 = new HashSet<string> { "R5", "RE", "RL", "U5", "UE", "UL" };
            var setR6 = new HashSet<string> { "R6", "RF", "RN", "U6", "UF", "UN" };

            if (setR1.Contains(cd)) return "R1";
            if (setR2.Contains(cd)) return "R2";
            if (setR3.Contains(cd)) return "R3";
            if (setR4.Contains(cd)) return "R4";
            if (setR5.Contains(cd)) return "R5";
            if (setR6.Contains(cd)) return "R6";
            return string.Empty;
        }

        /// <summary>
        /// Rotina que grava a interface (PERFORM 620-00-GRAVA-INTERFACE-DEA).
        /// Deve ser sobrescrita para persistir/emitir o registro.
        /// </summary>
        protected virtual void GravaInterfaceDea()
        {
            // Stub: override para gravar no destino (DB, fila, arquivo).
        }
    }
}