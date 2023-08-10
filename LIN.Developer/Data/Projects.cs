using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data;


public static class Projects
{



    #region Abstracciones


    /// <summary>
    /// Crea un nuevo proyecto
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(ProjectDataModel data)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await Create(data, context);
        context.CloseActions(connectionKey);
        return res;
    }



    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    public async static Task<ReadAllResponse<ProjectDataModel>> ReadAll(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;
    }


    /// <summary>
    /// Obtiene un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="profile">ID del perfil</param>
    public async static Task<ReadOneResponse<ProjectDataModel>> Read(int id, int profile)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Read(id, profile, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Elimina un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    public async static Task<ReadOneResponse<bool>> Delete(int id)
    {
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await Delete(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    #endregion



    /// <summary>
    /// Crea un nuevo proyecto
    /// </summary>
    /// <param name="data">Modelo de proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(ProjectDataModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {
            var res = await context.DataBase.Proyectos.AddAsync(data);
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
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<ProjectDataModel>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var projects = await Query.Project.ReadAll(id, context).ToListAsync();

            // Si hubo un error
            if (projects == null)
                return new(Responses.Undefined);

            // Retorna
            return new(Responses.Success, projects);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }


        return new();
    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<ProjectDataModel>> Read(int id, int profile, Conexión context)
    {

        // Ejecución
        try
        {

            // Obtiene el proyecto
            var project = await Query.Project.ReadOne(id, profile, context).FirstOrDefaultAsync();

            // Si hubo un error
            if (project == null)
                return new(Responses.NotRows);


            // Lista de IP
            var ips = await Query.FirewallRule.ReadAll(project.ID, context).ToListAsync();

            // Agrega las IP
            project.IPs = ips ?? new();

            // Retorna
            return new(Responses.Success, project);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }


        return new();
    }



    /// <summary>
    /// Obtiene si un proyecto acepta una regla firewall
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="ip">IP de la regla</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> HasFirewallFor(int id, string ip, Conexión context)
    {

        // Ejecución
        try
        {

            // Consulta
            var rules = await (from R in context.DataBase.FirewallRule
                               where R.ProjectID == id
                               where R.Status == FirewallRuleStatus.Normal
                               select R).ToListAsync();

            bool has = false;
            // Rules
            foreach (var rule in rules)
            {
                if (IP.IsIpInRange(ip, rule.IPInicio, rule.IPFinal))
                {
                    has = true;
                    break;
                }
            }

            // Si el proyecto no tiene firewall
            if (has)
                return new(Responses.Success, true);

            // Retorna el resultado
            return new(Responses.Unauthorized, false);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }


        return new();
    }



    /// <summary>
    /// Elimina un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> Delete(int id, Conexión context)
    {

        // Ejecución (Transacción)
        using (var transaction = context.DataBase.Database.BeginTransaction())
        {
            try
            {

                // Obtiene si hay reglas y la lista de reglas
                var project = await Query.Project.ReadOne(id, context).FirstOrDefaultAsync();

                // No existe
                if (project == null)
                {
                    transaction.Rollback();
                    return new(Responses.NotRows, false);
                }


                // Estado eliminado
                project.Estado = ProjectStatus.Deleted;


                // Obtiene las llaves
                var keys = await (from K in context.DataBase.ApiKeys
                                  where K.ProjectID == project.ID
                                  select K).ToListAsync();

                // Estado de las llaves
                foreach (var key in keys)
                    key.Status = ApiKeyStatus.Deleted;

                // Guarda los cambios
                context.DataBase.SaveChanges();
                transaction.Commit();


                // Retorna el resultado
                return new(Responses.Success, true);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ServerLogger.LogError(ex.Message);
            }
        }

        return new();
    }



}