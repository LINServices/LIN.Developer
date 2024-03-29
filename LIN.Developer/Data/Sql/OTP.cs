﻿namespace LIN.Developer.Data.Sql;


public static class OTP
{


    #region Abstracciones



    /// <summary>
    /// Crea un nuevo OTP
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(OTPDataModel data)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Create(data, context);
        context.CloseActions(connectionKey);
        return response;

    }



    /// <summary>
    /// Obtiene los OTP Validos
    /// </summary>
    /// <param name="id">ID del perfil</param>
    public async static Task<ReadAllResponse<OTPDataModel>> ReadAll(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Actualiza el estado
    /// </summary>
    /// <param name="id">ID del OTP</param>
    /// <param name="estado">Nuevo estado</param>
    public async static Task<ResponseBase> UpdateState(int id, OTPStatus estado)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await UpdateState(id, estado, context);
        context.CloseActions(connectionKey);
        return response;

    }



    /// <summary>
    /// Actualiza el estado
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="otp">Código OTP</param>
    public async static Task<ResponseBase> UpdateState(int id, string otp)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await UpdateState(id, otp, context);
        context.CloseActions(connectionKey);
        return response;

    }



    #endregion



    /// <summary>
    /// Crea un nuevo código OTP.
    /// </summary>
    /// <param name="data">Modelo</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(OTPDataModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {

            context.DataBase.Attach(data.Profile);
            var res = await context.DataBase.OTP.AddAsync(data);
            context.DataBase.SaveChanges();
            return new(Responses.Success, data.ID);
        }
        catch
        {
        }

        return new();
    }



    /// <summary>
    /// Obtiene los OTP activos de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<OTPDataModel>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {
            // retorna la lista
            var lista = await Query.OTP.ReadAll(id, context).ToListAsync();

            return new(Responses.Success, lista);
        }
        catch
        {
        }

        return new();
    }



    /// <summary>
    /// Actualiza el estado.
    /// </summary>
    /// <param name="id">ID del OTP</param>
    /// <param name="estado">Nuevo estado</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> UpdateState(int id, OTPStatus estado, Conexión context)
    {
        // Ejecución
        try
        {
            var modelo = await context.DataBase.OTP.FindAsync(id);

            if (modelo == null)
                return new(Responses.NotRows);


            modelo.Estado = estado;
            context.DataBase.SaveChanges();
            return new(Responses.Success);
        }
        catch
        {

        }

        return new();
    }



    /// <summary>
    /// Actualiza el estado
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="otp">Código OTP</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> UpdateState(int id, string otp, Conexión context)
    {
        // Ejecución
        try
        {

            var modelo = await Query.OTP.ReadAll(id, context).Where(T => T.OTP == otp).FirstOrDefaultAsync();

            if (modelo == null)
            {
                return new(Responses.NotRows);
            }

            modelo.Estado = OTPStatus.used;
            context.DataBase.SaveChanges();
            return new(Responses.Success);
        }
        catch (Exception ex)
        {

        }

        return new();
    }



}