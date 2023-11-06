using LIN.Developer.Data.Mongo.Query;

namespace LIN.Developer.Data.Mongo;


public static class Resources
{

    private const string Collection = "projects";

    #region Abstracciones


    /// <summary>
    /// Crea un nuevo proyecto
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(ResourceModel data)
    {
        // Obtiene la conexión
        var context = MongoService.GetOneConnection();

        var res = await Create(data, context);

        return res;
    }



    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    public async static Task<ReadAllResponse<ResourceModel>> ReadAll(int id)
    {
        // Obtiene la conexión
        var context = MongoService.GetOneConnection();

        var response = await ReadAll(id, context);

        return response;
    }



    /// <summary>
    /// Obtiene si un usuario tiene autorización para ver un proyecto y su contenido
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="profile">ID del perfil</param>
    public async static Task<ReadOneResponse<bool>> HaveAuthorization(int id, int profile)
    {
        // Obtiene la conexión
        var context = MongoService.GetOneConnection();

        var response = await HaveAuthorization(id, profile, context);

        return response;
    }



    /// <summary>
    /// Obtiene un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="profile">ID del perfil</param>
    public async static Task<ReadOneResponse<ResourceModel>> Read(string id, int profile)
    {
        // Obtiene la conexión
        var context = MongoService.GetOneConnection();

        var response = await Read(id, profile, context);

        return response;
    }



    /// <summary>
    /// Elimina un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    public async static Task<ReadOneResponse<bool>> Delete(int id)
    {
        var context = MongoService.GetOneConnection();
        var response = await Delete(id, context);

        return response;
    }



    #endregion



    /// <summary>
    /// Crea un nuevo proyecto.
    /// </summary>
    /// <param name="data">Modelo de proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(ResourceModel data, MongoService context)
    {

        // Ejecución
        try
        {

            var (id, success) = await context.Insert(data, Collection);

            if (!success)
                return new()
                {
                    Response = Responses.Undefined
                };

            return new()
            {
                LastUnique = id,
                Response = Responses.Success
            };
        }
        catch
        {
        }
        return new();
    }



    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<ResourceModel>> ReadAll(int id, MongoService context)
    {

        // Ejecución
        try
        {

            var projects = await Project.ReadAll(id, context).ToListAsync();

            // Si hubo un error
            if (projects == null)
                return new(Responses.Undefined);

            // Retorna
            return new(Responses.Success, projects);

        }
        catch
        {

        }


        return new();
    }




    /// <summary>
    /// Obtiene si un usuario tiene autorización para ver un proyecto y su contenido
    /// </summary>
    /// <param name="projectID">ID del proyecto</param>
    /// <param name="profileID">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> HaveAuthorization(int projectID, int profileID, MongoService context)
    {
        // Ejecución
        try
        {

            //var access = await (from P in context.DataBase.Proyectos
            //                    where P.ID == projectID && P.Profile.ID == profileID
            //                    where P.Estado == ProjectStatus.Normal
            //                    select P.ID).FirstOrDefaultAsync();


            //return (access <= 0) ? new(Responses.Unauthorized, false)
            //                     : new(Responses.Success, true);


        }
        catch (Exception ex)
        {

        }


        return new();
    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<ResourceModel>> Read(string id, int profile, MongoService context)
    {

        // Ejecución
        try
        {

            var project = await Project.ReadOne(id, profile, context).FirstOrDefaultAsync();

            // Si hubo un error
            if (project == null)
                return new(Responses.NotRows);


            // Lista de IP
            // var ips = await Query.FirewallRule.ReadAll(project.Id, context).ToListAsync();

            // Agrega las IP
            // project.IPs = ips ?? new();

            // Retorna
            return new(Responses.Success, project);

        }
        catch (Exception ex)
        {

        }


        return new();
    }



    /// <summary>
    /// Obtiene si un proyecto acepta una regla firewall
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="ip">IP de la regla</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> HasFirewallFor(int id, string ip, MongoService context)
    {

        // Ejecución
        try
        {

            //// Consulta
            //var rules = await (from R in context.DataBase.FirewallRules
            //                   where R.Project.ID == id
            //                   where R.Status == FirewallRuleStatus.Normal
            //                   select R).ToListAsync();

            //bool has = false;
            //// Rules
            //foreach (var rule in rules)
            //{
            //    if (IP.IsIpInRange(ip, rule.IPInicio, rule.IPFinal))
            //    {
            //        has = true;
            //        break;
            //    }
            //}

            //// Si el proyecto no tiene firewall
            //if (has)
            //    return new(Responses.Success, true);

            //// Retorna el resultado
            //return new(Responses.Unauthorized, false);

        }
        catch (Exception ex)
        {

        }


        return new();
    }



    /// <summary>
    /// Elimina un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> Delete(int id, MongoService context)
    {

        //// Ejecución (Transacción)
        //using (var transaction = context.DataBase.Database.BeginTransaction())
        //{
        //    try
        //    {

        //        //// Obtiene si hay reglas y la lista de reglas
        //        //var project = await Query.Project.ReadOne(id, context).FirstOrDefaultAsync();

        //        //// No existe
        //        //if (project == null)
        //        //{
        //        //    transaction.Rollback();
        //        //    return new(Responses.NotRows, false);
        //        //}


        //        //// Estado eliminado
        //        //project.Estado = ProjectStatus.Deleted;


        //        //// Obtiene las llaves
        //        //var keys = await (from K in context.DataBase.ApiKeys
        //        //                  where K.Project.ID == project.ID
        //        //                  select K).ToListAsync();

        //        //// Estado de las llaves
        //        //foreach (var key in keys)
        //        //    key.Status = ApiKeyStatus.Deleted;

        //        //// Guarda los cambios
        //        //context.DataBase.SaveChanges();
        //        //transaction.Commit();


        //        // Retorna el resultado
        //     //   return new(Responses.Success, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();

        //    }
        //}

        return new();
    }



}