using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Domain.Entities.Faturamento
{
    /// <summary>
    /// Tabela H1SF.ITD_ITMFATURADO
    /// </summary>
    public class ItemFaturado
    {
        public int ItdCdTRec { get; set; }
        public string ItdCdMercDst { get; set; }
        public DateTime ItdDtcSelFtrm { get; set; }
        public string ItdLgonFunc { get; set; } = string.Empty;
        
        /// <summary>
        /// ITD_ID_PTC_DSP - Identificador Protocolo Despacho
        /// </summary>
        public string? ItdIdPtcDsp { get; set; }
        
        // Campos para 535-00-ATUALIZA-PWS
        public int ItdIdCli { get; set; }
        public int ItdIdVol { get; set; }
        public int ItdIdEtiqRec { get; set; }
        public int ItdQPecaFtrd { get; set; }
        public string? ItdIdNfRef { get; set; }
        public int ItdIdMtz { get; set; }
        public string? ItdIdNf { get; set; }
        public string? ItdCdModTrsp { get; set; }
        
        // Campos para 875-00-MONTA-LOG-CAPS
        public string CodigoMercadoDestino { get; set; } = string.Empty;
        public string DataSelecaoFaturamento { get; set; } = string.Empty;
        public string LoginFuncionario { get; set; } = string.Empty;
        public string? IdNotaFiscalReferencia { get; set; }
        public int IdEtiquetaRecolhimento { get; set; }
        public DateTime? DataFaturaExportacao { get; set; } // ITD_DTC_FTR_EXP
        public string? NumeroFaturaExportacao { get; set; } // ITD_FTR_EXP
        public string? IdPedido { get; set; } // ITD_ID_PDD
        public string? IdClienteReferencia { get; set; } // ITD_ID_CLI_REF
        public string IdPeca { get; set; } = string.Empty; // ITD_ID_PECA
        public int QuantidadePecaFaturada { get; set; } // ITD_Q_PECA_FTRD
        public decimal PrecoFaturadoUnitarioEmissaoFiscal { get; set; } // ITD_PREC_FTRD_UNT_EMIF
        public decimal TaxaCambioUS { get; set; } // ITD_RATE_US
    }
}
