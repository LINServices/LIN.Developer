namespace LIN.Developer.Data.Query;


public static class ApiKeys
{


    /// <summary>
    /// (Query) Obtiene una api key
    /// </summary>
    /// <param name="key">String de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<KeyModel> ReadBy(string key, Conexión context)
    {

        //var query = (from A in context.DataBase.Keys
        //             where A.Key == key && A.Status != ApiKeyStatus.Deleted
        //             select A).Take(1);

        //return query;
        return null;
    }



    /// <summary>
    /// (Query) Obtiene una api key
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<KeyModel> ReadBy(int id, Conexión context)
    {

        //var query = (from A in context.DataBase.Keys
        //             where A.ID == id && A.Status != ApiKeyStatus.Deleted
        //             select A).Take(1);

        //return query;
        return null;
    }



    /// <summary>
    /// (Query) Obtiene las llaves asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<KeyModel> ReadAll(int id, Conexión context)
    {

        //var query = from AK in context.DataBase.Keys
        //            where AK.Project.ID == id && AK.Status != ApiKeyStatus.Deleted
        //            select AK;

        //return query;
        return null;
    }



    /// <summary>
    /// (Query) Obtiene el ID del proyecto al cual esta anclado una llave
    /// </summary>
    /// <param name="key">llave</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<int> GetProjectID(string key, Conexión context)
    {

        //var query = (from K in context.DataBase.Keys
        //             where K.Key == key && K.Status == ApiKeyStatus.Actived
        //             select K.ProjectId).Take(1);

        //return query;
        return null;
    }



}