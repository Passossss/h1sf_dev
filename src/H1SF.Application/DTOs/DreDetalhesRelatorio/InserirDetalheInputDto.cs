using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.DTOs.DreDetalhesRelatorio
{
    public class InserirDetalheInputDto
    {
        // Campos para validação bypass
        public string DreCnLnhRel { get; set; } = string.Empty;       // CB0002-DRE-CN-LNH-REL
        public string CdRegrFtrm { get; set; } = string.Empty;        // ST0001-CD-REGR-FTRM
        public string CdTPrd { get; set; } = string.Empty;           // CB0004-CD-T-PRD
        public string DreIdPrcpPtdLit { get; set; } = string.Empty;  // CB0002-DRE-ID-PRCP-PTD-LIT
        public string DreCdSqnPjl { get; set; } = string.Empty;      // CB0002-DRE-CD-SQN-PJL
        public string CdSqnPjlNf { get; set; } = string.Empty;       // WS01-CD-SQN-PJL-NF

        // Campos para inserção
        public string DreCdStm { get; set; } = string.Empty;         // CB0002-DRE-CD-STM
        public string DreDtcGrc { get; set; } = string.Empty;        // CB0002-DRE-DTC-GRC (YYYYMMDDHH24MISS)
        public string DreIdPrcp { get; set; } = string.Empty;        // CB0002-DRE-ID-PRCP
        public string DreCdSqnDct { get; set; } = string.Empty;      // CB0002-DRE-CD-SQN-DCT
    }
}
