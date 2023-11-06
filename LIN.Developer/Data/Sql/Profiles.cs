namespace LIN.Developer.Data.Sql;


public static class Profiles
{


    #region Abstracciones



    /// <summary>
    /// Crea un perfil 
    /// </summary>
    /// <param name="data">Modelo de desarrollador</param>
    public async static Task<CreateResponse> Create(ProfileDataModel data)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Create(data, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del usuario</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> ReadByUser(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadByUser(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del perfil</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> ReadBy(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await ReadBy(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene el ID y email de un perfil
    /// </summary>
    /// <param name="id">ID de la cuenta</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> FindByUser(int id)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await FindByUser(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene si una cuenta tiene perfil
    /// </summary>
    /// <param name="id">ID del usuario</param>
    public async static Task<ReadOneResponse<bool>> HasProfile(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await HasProfile(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Actualiza el estado de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="status">Nuevo estado</param>
    public async static Task<ReadOneResponse<bool>> UpdateState(int id, ProfileStatus status)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await UpdateState(id, status, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Actualiza el correo de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="email">Nuevo correo</param>
    public async static Task<ResponseBase> UpdateMail(int id, string email)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await UpdateMail(id, email, context);
        context.CloseActions(connectionKey);
        return response;
    }



    #endregion



    /// <summary>
    /// Crea un nuevo perfil
    /// </summary>
    /// <param name="data">Modelo de desarrollador</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(ProfileDataModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {
            var res = await context.DataBase.Profiles.AddAsync(data);
            context.DataBase.SaveChanges();

            return new(Responses.Success, data.ID);
        }
        catch (Exception ex)
        {
            if (ex.InnerException!.Message.Contains("Cannot insert duplicate key row in object"))
                return new(Responses.ExistAccount);


        }

        return new();
    }



    /// <summary>
    /// Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> ReadByUser(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var res = await Query.Profiles.ReadByUser(id, context).FirstOrDefaultAsync();

            // Si no existe el modelo
            if (res == null)
                return new(Responses.NotExistProfile);

            return new(Responses.Success, res);
        }
        catch (Exception ex)
        {

        }

        return new();
    }



    /// <summary>
    /// Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> ReadBy(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var res = await (from A in context.DataBase.Profiles
                             where A.ID == id
                             select A).FirstOrDefaultAsync();

            // Si no existe el modelo
            if (res == null)
                return new(Responses.NotExistProfile);

            return new(Responses.Success, res);
        }
        catch (Exception ex)
        {

        }

        return new();
    }



    /// <summary>
    /// Obtiene el perfil de desarrollador
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<ProfileDataModel>> FindByUser(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var res = await (from P in context.DataBase.Profiles
                             where P.AccountID == id
                             select new
                             {
                                 P.ID,
                                 Mail = P.Email
                             }).FirstOrDefaultAsync();

            // Si no existe el modelo
            if (res == null)
                return new(Responses.NotExistProfile);

            return new(Responses.Success, new()
            {
                ID = res.ID,
                Email = res.Mail
            });

        }
        catch (Exception ex)
        {

        }

        return new();
    }



    /// <summary>
    /// Obtiene si un perfil existe
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> HasProfile(int id, Conexión context)
    {
        // Ejecución
        try
        {

            var res = await Query.Profiles.CountProfiles(id, context).CountAsync();

            if (res == 0)
                return new(Responses.Success, false);
            else if (res == 1)
                return new(Responses.Success, true);

        }
        catch (Exception ex)
        {

        }

        return new(Responses.Undefined, false);
    }



    /// <summary>
    /// Actualiza el estado de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="status">Nuevo estado</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<bool>> UpdateState(int id, ProfileStatus status, Conexión context)
    {
        // Ejecución
        try
        {
            var modelo = await context.DataBase.Profiles.Where(T => T.ID == id).FirstOrDefaultAsync();

            if (modelo == null)
            {
                return new(Responses.NotExistProfile, false);
            }
            modelo.Estado = status;

            context.DataBase.SaveChanges();

            return new(Responses.Success, true);
        }
        catch (Exception ex)
        {

        }

        return new(Responses.Undefined, false);
    }



    /// <summary>
    /// Actualiza el correo de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="email">Nuevo correo</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> UpdateMail(int id, string email, Conexión context)
    {
        // Ejecución
        try
        {

            var modelo = await (from P in context.DataBase.Profiles
                                where P.ID == id
                                select P).FirstOrDefaultAsync();

            if (modelo == null)
                return new(Responses.NotExistProfile);

            modelo.Email = email;
            context.DataBase.SaveChanges();

            return new(Responses.Success);
        }
        catch (Exception ex)
        {

        }

        return new(Responses.Undefined);
    }



}