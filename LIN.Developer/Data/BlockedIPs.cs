using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data;


public class BlockedIPs
{


    #region Abstracciones


    /// <summary>
    /// Crea un registro de acceso bloqueado por el firewall
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(FirewallBlockLogDataModel data)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Create(data, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Crea un registro de acceso bloqueado por el firewall
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    public async static Task<ReadAllResponse<FirewallBlockLogDataModel>> ReadAll(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Elimina el historial de Firewall Bad Access
    /// </summary>
    /// <param name="id">ID del proyecto</param>
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
    /// Crea un registro de acceso bloqueado por el firewall
    /// </summary>
    /// <param name="data">Modelo</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(FirewallBlockLogDataModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {
            var res = await context.DataBase.FirewallBlockLogs.AddAsync(data);
            context.DataBase.SaveChanges();
            
            return new(Responses.Success, data.ID);
        }
        catch
        {
        }

        return new();
    }



    /// <summary>
    /// Obtiene los registros de acceso bloqueado por el firewall
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<FirewallBlockLogDataModel>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var res = await Query.BlockedIPs.ReadAll(id, context).ToListAsync();


            // Evalúa
            if (res == null)
            {
                return new();
            }

            return new(Responses.Success, res);
        }
        catch
        {
        }

        return new();
    }



    /// <summary>
    /// Elimina el historial de Firewall Bad Access
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> Delete(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var datos = await Query.BlockedIPs.ReadAll(id, context).ToListAsync();


            foreach(var dato in datos)
                dato.Estado = FirewallBlockStatus.Deleted;


            context.DataBase.UpdateRange(datos);
            context.DataBase.SaveChanges();

            return new(Responses.Success);
        }
        catch
        {
        }

        return new();
    }



}