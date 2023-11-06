using LIN.Types.Developer.Models;

namespace LIN.Developer.Data.Sql.Query;


internal static class Transactions
{

    /// <summary>
    /// Obtiene las transacciones de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto</param>
    public static IQueryable<TransactionDataModel> ReadAll(int id, Conexión context)
    {

        // Query
        var query = from TR in context.DataBase.Transactions
                    where TR.Profile.ID == id
                    orderby TR.Fecha descending
                    select TR;

        return query;

    }


}