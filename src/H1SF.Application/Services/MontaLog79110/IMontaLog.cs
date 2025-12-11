using System.Data;

namespace H1SF.Application.Services
{
    public interface IMontaLog
    {
        bool Executar(IDbConnection connection, MontaLog.Sf0001 sf0001, MontaLog.Cc0001 cc0001, MontaLog.Cc0002 cc0002, MontaLog.Wq01 wq01, out MontaLog.ItemLog itemLog, out MontaLog.St80014 st80014, out string errorMessage);
    }
}