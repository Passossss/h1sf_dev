using System.Data;

namespace H1SF.Application.Services
{
    public interface IMontaLog79010
    {
        bool Executar(IDbConnection connection, MontaLog79010.Sf0001 sf0001, string st0001CdFbr, MontaLog79010.Cc0001 cc0001, out MontaLog79010.St80014 st80014, out string errorMessage);
    }
}