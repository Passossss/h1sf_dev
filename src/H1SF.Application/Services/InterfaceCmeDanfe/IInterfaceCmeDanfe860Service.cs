using System;

namespace H1SF.Application.Services
{
    /// <summary>
    /// Interface para o módulo 860-00-INTERFACE-CME-DANFE.
    /// Todos os membros expostos como públicos (get/set) e o método Execute().
    /// </summary>
    public interface IInterfaceCmeDanfe860Service
    {
        // Entradas / contexto
        string SF0002ItdDtcSelFtrm { get; set; }
        string CC0001NumeroCtb5 { get; set; }
        string CC0001NumeroCtb6 { get; set; }
        string ST0001CdFbr { get; set; }
        string ST0006IdFrn { get; set; }
        string CC0001NopCodigo { get; set; }
        string SF0001PtdLgOnFunc { get; set; }
        string CC0001SerieSubserie3 { get; set; }
        string CC0001DhEmissaoBarra { get; set; }

        // Mensagem/Texto composto enviado
        string SF8003Sf80031 { get; set; }

        // Campos destino / gerados
        string WS35IdCorrIdLitSc { get; set; }
        string WS35IdCorrIdAlfSc { get; set; }
        string WS35IdCorrelId { get; set; }

        string SF80031CdFbr { get; set; }
        string SF80031CdNf { get; set; }
        string SF80031CdFrn { get; set; }
        string SF80031CdTMvto { get; set; }
        string SF80031CdTNf { get; set; }
        string SF80031CdNop { get; set; }
        string SF80031CdLgOnUsr { get; set; }
        string SF80031CdSrie { get; set; }
        string SF80031CdTMat { get; set; }
        string SF80031CdStaNf { get; set; }
        string SF80031DtEms { get; set; }

        string WQ01Sysdate { get; set; }
        string WQ01Sysdate8I { get; set; }

        string SF80031DtIncl { get; set; }
        string SF80031DtAtua { get; set; }

        string ACB50221ChvPrad { get; set; }
        int ACB50221TamMsg { get; set; }
        string ACB50221TxtMsg { get; set; }

        // Execução da seção principal
        void Execute();
    }
}