namespace LIN.Developer.Data.Query;


public static class FirewallRule
{


    /// <summary>
    /// Obtiene las reglas asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<FirewallRuleDataModel> ReadAll(int id, Conexión context)
    {

        // Lista de IP
        var query = from IP in context.DataBase.FirewallRule
                         where IP.ProjectID == id && IP.Status == FirewallRuleStatus.Normal
                         select IP;

        return query;
    }



}
