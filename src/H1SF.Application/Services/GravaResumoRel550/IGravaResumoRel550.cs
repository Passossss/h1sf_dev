using System.Data;

namespace H1SF.Application.Services
{
    public interface IGravaResumoRel550
    {
        void Executar(IDbConnection connection, string st0001CdRegrFtrm, string cb0004CdTPrd, string cb0001RreIdPrcpPtdLit, ref string ws31ChvComandoPjl, ref string cb0001RreCdSqnPjl, string cb0001RreCdStm, string cb0001RreDtcGrc, string cb0001RreIdPrcp, string cb0001RreCdSqnDct, string cb0001RreIdAuxImps1, string cb0001RreIdAuxImps2, string cb0001RreIdAuxImps3, string cb0001RreIdAuxImps4, string cb0001RreIdAuxImps5, string wi01RreIdAuxImps5, string cb0001RreIdRel, string cb0001RreIdImpr);
    }
}