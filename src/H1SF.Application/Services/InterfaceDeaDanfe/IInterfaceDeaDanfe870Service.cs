using System;

namespace H1SF.Application.Services.InterfaceDeaDanfe
{
    /// <summary>
    /// Interface para o módulo 870-00-INTERFACE-DEA-DANFE.
    /// Expõe entradas principais, campos de saída e a operação principal Execute().
    /// </summary>
    public interface IInterfaceDeaDanfe870Service
    {
        // Entradas / contexto
        string SF0005SftCdTRec { get; set; }
        string WS36CdMercDst { get; set; }
        string SF0005SftIcFtrmTrgd { get; set; }

        string CC0001CfopCodigoLog { get; set; }

        int WS01QTtlItemFat { get; set; }

        string SF0002ItdCdTRec { get; set; }
        string SF0002ItdCdModTrspLog { get; set; }
        string CC0005RazaoSocial { get; set; }

        string CC0001PrecoTotalMDea { get; set; }
        string CC0001VlTotalBaseIpiDea { get; set; }
        string CC0001VlTotalIpiDea { get; set; }
        string CC0001VlTotalBaseIcmsDea { get; set; }
        string CC0001VlTotalIcmsDea { get; set; }
        string CC0001VlTotalBaseStfDea { get; set; }
        string CC0001VlTotalStfDea { get; set; }
        string CC0001VlTotalContabilDea { get; set; }
        string CC0001PesoBrutoKgDea { get; set; }
        string CC0001VlFreteDea { get; set; }
        string CC0001VlOutrasDespesasCtb { get; set; }

        string CC0001VlAjustePrecoTotalM { get; set; }
        string CC0001VlDescontoLog { get; set; }

        string SF0002ItdFtrExpLog { get; set; }

        string SF0001PtdIdMtz { get; set; }

        // Saídas / campos gerados pela rotina
        string SE0001IdeIdTItf { get; set; }
        string SE0001IdeTxtMsg { get; set; }
        long SE0001IdeNumTReg { get; set; }

        int SE90051QtItem { get; set; }
        string SE90051CdTRec { get; set; }
        string SE90051CdModTrsp { get; set; }
        string SE90051NmTrsr { get; set; }
        string SE90051VttlMrcdNf { get; set; }
        string SE90051VttlBsIpiNf { get; set; }
        string SE90051VttlIpiNf { get; set; }
        string SE90051VttlBsIcmsNf { get; set; }
        string SE90051VttlIcmsNf { get; set; }
        string SE90051VttlBsIcmsSubs { get; set; }
        string SE90051VttlIcmsSubs { get; set; }
        string SE90051VttlFtrdNf { get; set; }
        string SE90051VttlPesoNf { get; set; }
        string SE90051VttlFrtNf { get; set; }
        string SE90051VttlServNf { get; set; }
        string SE90051VttlDsctNf { get; set; }
        string SE90051IdFtrExp { get; set; }
        string SE90051CdS9xEsp { get; set; }
        string SE90051TxtMsg { get; set; }

        string SE90251TxtMsg { get; set; }

        string WS01FimDeaProcesso { get; set; }

        // Execução da seção principal
        void Execute();
    }
}