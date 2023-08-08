namespace LIN.Developer.Data;


public static class ApiKeyUses
{


    #region Abstracciones


    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso</param>
    public async static Task<CreateResponse> GenerateUses(ApiKeyUsesDataModel data)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await GenerateUses(data, context);
        context.CloseActions(connectionKey);
        return response;

    }



    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso</param>
    /// <param name="key">String de la llave</param>
    public async static Task<CreateResponse> GenerateUses(ApiKeyUsesDataModel data, string key)
    {
        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await GenerateUses(data, key, context);
        context.CloseActions(connectionKey);
        return response;
    }



    #endregion



    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> GenerateUses(ApiKeyUsesDataModel data, Conexión context)
    {

        // Modelo
        data.ID = 0;

        // Ejecución (Transacción)
        using (var dbTransaction = context.GetTransaction())
        {
            try
            {

                // Obtiene la llave
                var key = await context.DataBase.ApiKeys.FindAsync(data.KeyID);

                // Evalúa la llave
                if (key == null || key.Status != ApiKeyStatus.Actived)
                {
                    dbTransaction.Rollback();
                    return new(Responses.InvalidApiKey);
                }

                // Obtiene el proyecto
                var project = await context.DataBase.Proyectos.FindAsync(key.ProjectID);

                // Si no existe
                if (project == null)
                {
                    dbTransaction.Rollback();
                    return new(Responses.NotExistProfile);
                }

                var profile = await context.DataBase.Profiles.FindAsync(project.ProfileID);

                if (profile == null)
                {
                    dbTransaction.Rollback();
                    return new(Responses.DontHavePermissions);
                }

                data.Valor = Pricing.Discount(data.Valor, profile.Discont);

                // Si no hay créditos
                if (profile == null || profile.Credito - data.Valor < 0m)
                {
                    dbTransaction.Rollback();
                    return new(Responses.DontHaveCredits);
                }


                // Nueva transacción
                var transaction = new TransactionDataModel()
                {
                    Description = "Usado en un servicio LIN",
                    Fecha = DateTime.Now,
                    ID = 0,
                    ProfileID = profile.ID,
                    Tipo = TransactionTypes.UsedService,
                    Valor = Pricing.ToNegative(data.Valor)
                };


                // Transacción
                var responseTransaction = await Transactions.Generate(transaction, context, false);

                if (responseTransaction.Response != Responses.Success)
                {
                    dbTransaction.Rollback();
                    return new();
                }

                data.TransactionID = responseTransaction.LastID;

                // Agrega el uso
                await context.DataBase.ApikeyUses.AddAsync(data);

                // Guarda los cambios
                context.DataBase.SaveChanges();

                // Envía la transacción
                dbTransaction.Commit();

                // Cierra la conexión
                return new(Responses.Success, data.ID);

            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert explicit value for identity"))
                {

                }

                ServerLogger.LogError("AJ" + ex.InnerException);
            }
        }

        return new();
    }



    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso</param>
    /// <param name="key">String de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> GenerateUses(ApiKeyUsesDataModel data, string key, Conexión context)
    {

        // Ejecución
        try
        {

            // Obtiene la llave
            var apiKey = await Query.ApiKeys.ReadBy(key, context).Select(T => T.ID).FirstOrDefaultAsync();

            // No existe
            if (apiKey <= 0)
            {
                return new(Responses.InvalidApiKey);
            }

            data.KeyID = apiKey;
            return await GenerateUses(data, context);
        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new();
    }


}