using System;

namespace H1SF.Application.Services 
{
    /// <summary>
    /// Tradução isolada do trecho COBOL 860-00-INTERFACE-CME-DANFE para C#.
    /// Arquivo independente; consultas/produção MQ são métodos protected virtual para override/injeção.
    /// </summary>
    public class InterfaceCmeDanfe860Service : IInterfaceCmeDanfe860Service
    {
        // Entradas / contexto
        public string SF0002ItdDtcSelFtrm { get; set; } = string.Empty;
        public string CC0001NumeroCtb5 { get; set; } = string.Empty;
        public string CC0001NumeroCtb6 { get; set; } = string.Empty;
        public string ST0001CdFbr { get; set; } = string.Empty;
        public string ST0006IdFrn { get; set; } = string.Empty;
        public string CC0001NopCodigo { get; set; } = string.Empty;
        public string SF0001PtdLgOnFunc { get; set; } = string.Empty;
        public string CC0001SerieSubserie3 { get; set; } = string.Empty;
        public string CC0001DhEmissaoBarra { get; set; } = string.Empty;

        // Valores monetários / textos compostos usados no envio
        public string SF8003Sf80031 { get; set; } = string.Empty;

        // Campos destino (SE80031 / ACB50221 / WS35)
        public string WS35IdCorrIdLitSc { get; set; } = string.Empty;
        public string WS35IdCorrIdAlfSc { get; set; } = string.Empty;
        public string WS35IdCorrelId { get; set; } = string.Empty;

        public string SF80031CdFbr { get; set; } = string.Empty;
        public string SF80031CdNf { get; set; } = string.Empty;
        public string SF80031CdFrn { get; set; } = string.Empty;
        public string SF80031CdTMvto { get; set; } = string.Empty;
        public string SF80031CdTNf { get; set; } = string.Empty;
        public string SF80031CdNop { get; set; } = string.Empty;
        public string SF80031CdLgOnUsr { get; set; } = string.Empty;
        public string SF80031CdSrie { get; set; } = string.Empty;
        public string SF80031CdTMat { get; set; } = string.Empty;
        public string SF80031CdStaNf { get; set; } = string.Empty;
        public string SF80031DtEms { get; set; } = string.Empty;

        // Corrige: implementa setters públicos para WQ01Sysdate e WQ01Sysdate8I conforme exigido pela interface
        public string WQ01Sysdate
        {
            get => _wq01Sysdate;
            set => _wq01Sysdate = value;
        }
        public string WQ01Sysdate8I
        {
            get => _wq01Sysdate8I;
            set => _wq01Sysdate8I = value;
        }

        // Campos privados para backing das propriedades
        private string _wq01Sysdate = string.Empty;
        private string _wq01Sysdate8I = string.Empty;

        public string SF80031DtIncl { get; set; } = string.Empty;
        public string SF80031DtAtua { get; set; } = string.Empty;

        public string ACB50221ChvPrad { get; set; } = string.Empty;
        public int ACB50221TamMsg { get; set; }
        public string ACB50221TxtMsg { get; set; } = string.Empty;

        public string WS35IdCorrIdLitScOut => WS35IdCorrIdLitSc;
        public string WS35IdCorrIdAlfScOut => WS35IdCorrIdAlfSc;

        public InterfaceCmeDanfe860Service() { }

        /// <summary>
        /// Executa a lógica traduzida do COBOL 860-00.
        /// </summary>
        public virtual void Execute()
        {
            // MOVE SF0002-ITD-DTC-SEL-FTRM TO WS35-ID-CORR-ID-LIT-SC.
            WS35IdCorrIdLitSc = SF0002ItdDtcSelFtrm;

            // MOVE CC0001-NUMERO-CTB-5 TO WS35-ID-CORR-ID-ALF-SC.
            WS35IdCorrIdAlfSc = CC0001NumeroCtb5;

            // INSPECT WS35-ID-CORREL-ID CONVERTING SPACES TO ZEROS.
            // Monta correl composto e converte espaços para zeros
            WS35IdCorrelId = (WS35IdCorrIdLitSc + WS35IdCorrIdAlfSc) ?? string.Empty;
            WS35IdCorrelId = ConvertSpacesToZeros(WS35IdCorrelId);

            // MOVE ST0001-CD-FBR TO SF80031-CD-FBR.
            SF80031CdFbr = ST0001CdFbr;

            // MOVE CC0001-NUMERO-CTB-6 TO SF80031-CD-NF.
            SF80031CdNf = CC0001NumeroCtb6;

            // MOVE ST0006-ID-FRN TO SF80031-CD-FRN.
            SF80031CdFrn = ST0006IdFrn;

            // MOVE 'NFS' TO SF80031-CD-T-MVTO.
            SF80031CdTMvto = "NFS";

            // MOVE 'NF' TO SF80031-CD-T-NF.
            SF80031CdTNf = "NF";

            // MOVE CC0001-NOP-CODIGO TO SF80031-CD-NOP.
            SF80031CdNop = CC0001NopCodigo;

            // MOVE SF0001-PTD-LGON-FUNC TO SF80031-CD-LGON-USR.
            SF80031CdLgOnUsr = SF0001PtdLgOnFunc;

            // MOVE CC0001-SERIE-SUBSERIE-3 TO SF80031-CD-SRIE.
            SF80031CdSrie = CC0001SerieSubserie3;

            // MOVE 'M' TO SF80031-CD-T-MAT.
            SF80031CdTMat = "M";

            // MOVE 'A' TO SF80031-CD-STA-NF.
            SF80031CdStaNf = "A";

            // MOVE CC0001-DH-EMISSAO-BARRA TO SF80031-DT-EMS.
            SF80031DtEms = CC0001DhEmissaoBarra;

            // EXEC SQL SELECT TO_CHAR(SYSDATE,'YYYY/MM/DD'), TO_CHAR(SYSDATE,'YYYYMMDD') INTO :WQ01-SYSDATE, :WQ01-SYSDATE-8-I FROM DUAL WHERE ROWNUM = 1
            (WQ01Sysdate, WQ01Sysdate8I) = GetSysDate();

            // MOVE WQ01-SYSDATE-8-I TO SF80031-DT-INCL SF80031-DT-ATUA.
            SF80031DtIncl = WQ01Sysdate8I;
            SF80031DtAtua = WQ01Sysdate8I;

            // MOVE '08' TO ACB50221-CHV-PRAD.
            ACB50221ChvPrad = "08";

            // MOVE 107 TO ACB50221-TAM-MSG.
            ACB50221TamMsg = 107;

            // MOVE SF8003-SF80031 TO ACB50221-TXT-MSG.
            ACB50221TxtMsg = SF8003Sf80031;

            // PERFORM 555-00-GRAVA-MENSAGEM-MQ.
            GravaMensagemMq();
        }

        // -------------------------
        // Métodos substituíveis / stubs
        // -------------------------

        /// <summary>
        /// Obtém SYSDATE formatado (TO_CHAR(SYSDATE,'YYYY/MM/DD'), TO_CHAR(SYSDATE,'YYYYMMDD')).
        /// Override para usar query ao DB se necessário.
        /// </summary>
        protected virtual (string sysdate, string sysdate8) GetSysDate()
        {
            var now = DateTime.Now;
            return (now.ToString("yyyy/MM/dd"), now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Grava/produz a mensagem na fila MQ (PERFORM 555-00-GRAVA-MENSAGEM-MQ).
        /// Override para implementação concreta.
        /// </summary>
        protected virtual void GravaMensagemMq()
        {
            // stub: implementar envio para MQ/rota apropriada.
        }

        /// <summary>
        /// Substitui INSPECT ... CONVERTING SPACES TO ZEROS.
        /// </summary>
        protected static string ConvertSpacesToZeros(string s)
        {
            if (s is null) return string.Empty;
            return s.Replace(' ', '0');
        }
    }
}