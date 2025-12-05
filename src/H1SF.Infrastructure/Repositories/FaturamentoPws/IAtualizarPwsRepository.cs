using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Infrastructure.Repositories.FaturamentoPws
{

    public interface IAtualizarPwsRepository
    {
        Task<bool> DeveExecutarAtualizacaoAsync(string? sftIcNaczIcpnBt, string? sftCdTRec);
        Task<List<ItemFaturadoAgrupadoDto>> ObterItensAgrupadosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm);
        Task<bool> AtualizarItemRecolhimentoAsync(int idEtiqRec, int qPecaRec);
        Task<int?> ObterSomaQuantidadesItemAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm, int idEtiqRec);
        Task<List<VolumeInfoDto>> ObterVolumesDistintosAsync(string cdMercDst, DateTime dtcSelFtrm, string lgonFunc, string faseFtrm);
        Task<bool> AtualizarIdDocumentoFiscalAsync(int idVol, string idNf);
        Task<bool> AtualizarVolumeFaturadoAsync(int idVol);
        Task<bool> VerificarTodosItensFinalizadosAsync(int idVol);
    }

    public class ItemFaturadoAgrupadoDto
    {
        public int ItdIdCli { get; set; }
        public int ItdIdVol { get; set; }
        public int ItdIdEtiqRec { get; set; }
        public int SomaQuantidade { get; set; }
    }

    public class VolumeInfoDto
    {
        public int ItdIdMtz { get; set; }
        public int ItdIdCli { get; set; }
        public int ItdIdVol { get; set; }
        public string? ItdIdNf { get; set; }
        public string? ItdCdModTrsp { get; set; }
    }
}
