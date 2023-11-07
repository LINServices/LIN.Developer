namespace LIN.Developer.Data.Mongo.Query;


public static class Project
{


    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ResourceModel> ReadAll(int id, MongoService context)
    {

        var query = from P in context.Context.Projects
                    where P.ProfileId == id
                    && P.Status == ProjectStatus.Normal
                    select new ResourceModel
                    {
                        Status = P.Status,
                        Id = P.Id,
                        Name = P.Name,
                        ProfileId = P.ProfileId,
                        Type = P.Type
                    };

        //return query;
        return query;
    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ResourceModel> ReadOne(string id, MongoService context)
    {

        // Consulta
        var query = (from P in context.Context.Projects
                    where P.Id == new ObjectId(id)
                    && P.Status == ProjectStatus.Normal
                    select P).Take(1);

        return query;

    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ResourceModel> ReadOne(int id, MongoService context)
    {

        //var query = (from P in context.DataBase.Proyectos
        //             where P.ID == id && P.Estado == ProjectStatus.Normal
        //             select P).Take(1);

        //return query;
        return null;
    }





}