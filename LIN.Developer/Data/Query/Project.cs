namespace LIN.Developer.Data.Query;


public static class Project
{


    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ProjectDataModel> ReadAll(int id, Conexión context)
    {

        var query = from P in context.DataBase.Proyectos
                    where P.Profile.ID == id && P.Estado == ProjectStatus.Normal
                    select P;

        return query;
    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ProjectDataModel> ReadOne(int id, int profile, Conexión context)
    {

        // Consulta
        var query = ReadAll(profile, context).Where(T => T.ID == id).Take(1);

        return query;

    }



    /// <summary>
    /// Obtiene un proyecto y sus reglas
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ProjectDataModel> ReadOne(int id, Conexión context)
    {

        var query = (from P in context.DataBase.Proyectos
                     where P.ID == id && P.Estado == ProjectStatus.Normal
                     select P).Take(1);

        return query;

    }





}