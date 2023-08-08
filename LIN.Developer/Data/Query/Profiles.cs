namespace LIN.Developer.Data.Query;


public static class Profiles
{

    /// <summary>
    /// (Query) Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ProfileDataModel> ReadByUser(int id, Conexión context)
    {

        var query = (from D in context.DataBase.Profiles
                    where D.UserID == id && D.Estado == ProfileStatus.Normal
                    select D).Take(1);

        return query;

    }



    /// <summary>
    /// (Query) Cuenta los perfiles
    /// </summary>
    /// <param name="id">ID del usuario (Cuenta principal)</param>
    /// <param name="context">Contexto de conexión</param>
    public static IQueryable<ProfileDataModel> CountProfiles(int id, Conexión context)
    {

        var query = (from D in context.DataBase.Profiles
                    where D.UserID == id
                     select D).Take(1);

        return query;

    }


}
