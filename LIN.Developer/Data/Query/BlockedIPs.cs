using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data.Query;


public class BlockedIPs
{


    /// <summary>
    /// (Query) Obtiene los registros de acceso bloqueado por el firewall
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<FirewallBlockLogDataModel> ReadAll(int id, Conexión context)
    {

        // Consulta
        var query = from FR in context.DataBase.FirewallBlockLogs
                    where FR.ProyectoID == id && FR.Estado == FirewallBlockStatus.Normal
                    select FR;

        return query;
    }


}