namespace LIN.Developer.Data;


public static class FirewallRules
{


    #region Abstracciones


    /// <summary>
    /// Crea una nueva regla firewall
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(FirewallRuleDataModel data)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Create(data, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene las reglas asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    public async static Task<ReadAllResponse<FirewallRuleDataModel>> ReadAll(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Elimina una regla de firewall
    /// </summary>
    /// <param name="id">ID de la regla</param>
    public async static Task<ResponseBase> Delete(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await Delete(id, context);
        context.CloseActions(connectionKey);
        return response;
    }


    #endregion



    /// <summary>
    /// Crea una nueva regla de firewall
    /// </summary>
    /// <param name="data">Modelo de la regla</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(FirewallRuleDataModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {
            var res = await context.DataBase.FirewallRule.AddAsync(data);
            context.DataBase.SaveChanges();

            return new(Responses.Success, data.ID);
        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new();
    }



    /// <summary>
    /// Obtiene las reglas asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<FirewallRuleDataModel>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var rules = await Query.FirewallRule.ReadAll(id, context).ToListAsync();

            // Comprueba errores
            if (rules == null)
            {
                return new(Responses.Undefined);
            }

            // Retorna
            return new(Responses.Success, rules);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new();
    }



    /// <summary>
    /// Elimina una regla firewall
    /// </summary>
    /// <param name="id">ID de la regla</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> Delete(int id, Conexión context)
    {

        // Ejecución
        try
        {

            // IP
            var ip = await (from IP in context.DataBase.FirewallRule
                            where IP.ID == id
                            select IP).FirstOrDefaultAsync();

            // Comprueba errores
            if (ip == null)
            {
                return new(Responses.Undefined);
            }

            // Cambia el estado
            ip.Status = FirewallRuleStatus.Deleted;
            context.DataBase.SaveChanges();


            // Retorna
            return new(Responses.Success);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new();
    }


}
