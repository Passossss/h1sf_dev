using H1SF.Application.Services.InterfaceDeaDanfe;
using System;
using System.Globalization;

namespace H1SF.Application.Services.InterfaceDeaDanfe870
{
    /// <summary>
    /// Tradução isolada do trecho COBOL 870-00-INTERFACE-DEA-DANFE para C#.
    /// Arquivo independente; operações externas (gravação) são métodos protegidos virtuais para override/injeção.
    /// </summary>
    public class InterfaceDeaDanfe870Service : IInterfaceDeaDanfe870Service
    {
        // Entradas / contexto (mantive nomes COBOL-like para rastreabilidade)
        public string SF0005SftCdTRec { get; set; } = string.Empty;
        public string WS36CdMercDst { get; set; } = string.Empty;
        public string SF0005SftIcFtrmTrgd { get; set; } = string.Empty;

        public string CC0001CfopCodigoLog { get; set; } = string.Empty;

        public int WS01QTtlItemFat { get; set; }

        public string SF0002ItdCdTRec { get; set; } = string.Empty;
        public string SF0002ItdCdModTrspLog { get; set; } = string.Empty;
        public string CC0005RazaoSocial { get; set; } = string.Empty;

        public string CC0001PrecoTotalMDea { get; set; } = string.Empty;
        public string CC0001VlTotalBaseIpiDea { get; set; } = string.Empty;
        public string CC0001VlTotalIpiDea { get; set; } = string.Empty;
        public string CC0001VlTotalBaseIcmsDea { get; set; } = string.Empty;
        public string CC0001VlTotalIcmsDea { get; set; } = string.Empty;
        public string CC0001VlTotalBaseStfDea { get; set; } = string.Empty;
        public string CC0001VlTotalStfDea { get; set; } = string.Empty;
        public string CC0001VlTotalContabilDea { get; set; } = string.Empty;
        public string CC0001PesoBrutoKgDea { get; set; } = string.Empty;
        public string CC0001VlFreteDea { get; set; } = string.Empty;
        public string CC0001VlOutrasDespesasCtb { get; set; } = string.Empty;

        public string CC0001VlAjustePrecoTotalM { get; set; } = string.Empty;
        public string CC0001VlDescontoLog { get; set; } = string.Empty;

        public string SF0002ItdFtrExpLog { get; set; } = string.Empty;

        public string SF0001PtdIdMtz { get; set; } = string.Empty;

        // Campos destino / SE* (mantidos nomes COBOL-like)
        public string SE0001IdeIdTItf { get; set; } = string.Empty;
        public string SE0001IdeTxtMsg { get; set; } = string.Empty;
        public long SE0001IdeNumTReg { get; set; }

        public int SE90051QtItem { get; set; }
        public string SE90051CdTRec { get; set; } = string.Empty;
        public string SE90051CdModTrsp { get; set; } = string.Empty;
        public string SE90051NmTrsr { get; set; } = string.Empty;
        public string SE90051VttlMrcdNf { get; set; } = string.Empty;
        public string SE90051VttlBsIpiNf { get; set; } = string.Empty;
        public string SE90051VttlIpiNf { get; set; } = string.Empty;
        public string SE90051VttlBsIcmsNf { get; set; } = string.Empty;
        public string SE90051VttlIcmsNf { get; set; } = string.Empty;
        public string SE90051VttlBsIcmsSubs { get; set; } = string.Empty;
        public string SE90051VttlIcmsSubs { get; set; } = string.Empty;
        public string SE90051VttlFtrdNf { get; set; } = string.Empty;
        public string SE90051VttlPesoNf { get; set; } = string.Empty;
        public string SE90051VttlFrtNf { get; set; } = string.Empty;
        public string SE90051VttlServNf { get; set; } = string.Empty;
        public string SE90051VttlDsctNf { get; set; } = string.Empty;
        public string SE90051IdFtrExp { get; set; } = string.Empty;
        public string SE90051CdS9xEsp { get; set; } = string.Empty;
        public string SE90051TxtMsg { get; set; } = string.Empty;

        public string SE90251TxtMsg { get; set; } = string.Empty;

        public string WS01FimDeaProcesso { get; set; } = string.Empty;

        public InterfaceDeaDanfe870Service() { }

        /// <summary>
        /// Executa a seção principal do COBOL (870-00...).
        /// </summary>
        public virtual void Execute()
        {
            // IF SF0005-SFT-CD-T-REC EQUAL 'XX' GO TO 870-99-INTERFACE-EXIT.
            if (string.Equals(SF0005SftCdTRec, "XX", StringComparison.Ordinal))
                return;

            IdentificaInterface();
            CarregaCampos();
            FinalizaInterface();
            CarregaCamposFim();
        }

        // 870-10-IDENTIFICA-INTERFACE
        protected virtual void IdentificaInterface()
        {
            if (string.Equals(WS36CdMercDst, "D", StringComparison.Ordinal))
            {
                if (!string.Equals(SF0005SftIcFtrmTrgd, "S", StringComparison.Ordinal))
                    SE0001IdeIdTItf = "NOTA_FISCAL_DE_VENDA_DOM";
                else
                    SE0001IdeIdTItf = "NOTA_FISCAL_DE_VENDA_TEF";
            }
            else
            {
                SE0001IdeIdTItf = "NOTA_FISCAL_DE_REEXPORTACAO";
            }
        }

        // 870-20-CARREGA-CAMPOS
        protected virtual void CarregaCampos()
        {
            // INSPECT CC0001-CFOP-CODIGO-LOG CONVERTING SPACES TO ZEROS.
            CC0001CfopCodigoLog = ConvertSpacesToZeros(CC0001CfopCodigoLog);

            // MOVE WS01-ID-NUM-T-REG TO SE90051-QT-ITEM.
            SE90051QtItem = WS01QTtlItemFat;

            // MOVE SF0002-ITD-CD-T-REC TO SE90051-CD-T-REC.
            SE90051CdTRec = SF0002ItdCdTRec;

            // MOVE SF0002-ITD-CD-MOD-TRSP-LOG TO SE90051-CD-MOD-TRSP.
            SE90051CdModTrsp = SF0002ItdCdModTrspLog;

            // MOVE CC0005-RAZAO-SOCIAL TO SE90051-NM-TRSR.
            SE90051NmTrsr = CC0005RazaoSocial;

            // Valores monetários / outros
            SE90051VttlMrcdNf = CC0001PrecoTotalMDea;
            SE90051VttlBsIpiNf = CC0001VlTotalBaseIpiDea;
            SE90051VttlIpiNf = CC0001VlTotalIpiDea;
            SE90051VttlBsIcmsNf = CC0001VlTotalBaseIcmsDea;
            SE90051VttlIcmsNf = CC0001VlTotalIcmsDea;
            SE90051VttlBsIcmsSubs = CC0001VlTotalBaseStfDea;
            SE90051VttlIcmsSubs = CC0001VlTotalStfDea;
            SE90051VttlFtrdNf = CC0001VlTotalContabilDea;
            SE90051VttlPesoNf = CC0001PesoBrutoKgDea;
            SE90051VttlFrtNf = CC0001VlFreteDea;
            SE90051VttlServNf = CC0001VlOutrasDespesasCtb;

            // desconto/ajuste depende de destino
            if (string.Equals(WS36CdMercDst, "E", StringComparison.Ordinal))
                SE90051VttlDsctNf = CC0001VlAjustePrecoTotalM;
            else
                SE90051VttlDsctNf = CC0001VlDescontoLog;

            // MOVE SF0002-ITD-FTR-EXP-LOG TO SE90051-ID-FTR-EXP.
            SE90051IdFtrExp = SF0002ItdFtrExpLog;

            // IF SF0001-PTD-ID-MTZ EQUAL 'U05A' MOVE 'A' TO SE90051-CD-S9X-ESPC ELSE SPACES
            if (string.Equals(SF0001PtdIdMtz, "U05A", StringComparison.Ordinal))
                SE90051CdS9xEsp = "A";
            else
                SE90051CdS9xEsp = string.Empty;

            // IF WS36-CD-MERC-DST EQUAL 'D' MOVE SE90051-TXT-MSG TO SE0001-IDE-TXT-MSG
            // ELSE MOVE SE90051-TXT-MSG TO SE90251-TXT-MSG & MOVE SE90251-TXT-MSG TO SE0001-IDE-TXT-MSG.
            if (string.Equals(WS36CdMercDst, "D", StringComparison.Ordinal))
            {
                SE0001IdeTxtMsg = SE90051TxtMsg;
            }
            else
            {
                SE90251TxtMsg = SE90051TxtMsg;
                SE0001IdeTxtMsg = SE90251TxtMsg;
            }

            // PERFORM 620-00-GRAVA-INTERFACE-DEA.
            GravaInterfaceDea();
        }

        // 870-30-FINALIZA-INTERFACE
        protected virtual void FinalizaInterface()
        {
            // Reaplica identificação (mesma lógica do IDENTIFICA-INTERFACE)
            IdentificaInterface();

            // MOVE 999999999 TO SE0001-IDE-NUM-T-REG
            SE0001IdeNumTReg = 999_999_999L;
        }

        // 870-40-CARREGA-CAMPOS-FIM
        protected virtual void CarregaCamposFim()
        {
            WS01FimDeaProcesso = "H1SF0033";
            GravaInterfaceDea();
        }

        // -------------------------
        // Métodos substituíveis / stubs
        // -------------------------

        /// <summary>
        /// Persiste ou encaminha o registro de interface (PERFORM 620-00-GRAVA-INTERFACE-DEA).
        /// Override para gravar no DB/fila/arquivo conforme ambiente.
        /// </summary>
        protected virtual void GravaInterfaceDea()
        {
            // stub: sobrescreva para persistir/emitir o registro
        }

        /// <summary>
        /// Substitui o comportamento COBOL INSPECT ... CONVERTING SPACES TO ZEROS.
        /// </summary>
        protected static string ConvertSpacesToZeros(string s)
        {
            if (s == null) return string.Empty;
            return s.Replace(' ', '0');
        }
    }
}