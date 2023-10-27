
namespace LIN.Developer.Data;


public static class ApiKeyUses
{


    #region Abstracciones


    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso</param>
    public async static Task<CreateResponse> GenerateUses(BillingItemModel data)
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
    public async static Task<CreateResponse> GenerateUses(BillingItemModel data, string key)
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
    public async static Task<CreateResponse> GenerateUses(BillingItemModel data, Conexión context)
    {

        // Modelo
        data.ID = 0;

        // Ejecución (Transacción)
        using (var dbTransaction = context.GetTransaction())
        {
            //try
            //{

            //    var key = await new MongoDBConnection().DBConnection.GetDatabase("Cluster0").GetCollection<KeyModel>("keys").Find(t => t.ID == data.KeyUId).FirstOrDefaultAsync();

            //    // Evalúa la llave
            //    if (key == null || key.Status != ApiKeyStatus.Actived)
            //    {
            //        dbTransaction.Rollback();
            //        return new(Responses.InvalidApiKey);
            //    }

            //    // Obtiene el proyecto
            //    var project = await context.DataBase.Proyectos.FindAsync(key.Project.ID);

            //    // Si no existe
            //    if (project == null)
            //    {
            //        dbTransaction.Rollback();
            //        return new(Responses.NotExistProfile);
            //    }

            //    var profile = await context.DataBase.Profiles.FindAsync(project.Profile.ID);

            //    if (profile == null)
            //    {
            //        dbTransaction.Rollback();
            //        return new(Responses.Unauthorized);
            //    }

            //    data.Valor = Pricing.Discount(data.Valor, profile.Discont);

            //    // Si no hay créditos
            //    if (profile == null || profile.Credito - data.Valor < 0m)
            //    {
            //        dbTransaction.Rollback();
            //        return new(Responses.WithoutCredits);
            //    }


            //    // Nueva transacción
            //    var transaction = new TransactionDataModel()
            //    {
            //        Description = "Usado en un servicio LIN",
            //        Fecha = DateTime.Now,
            //        ID = 0,
            //        Profile = new()
            //        {
            //            ID = profile.ID,
            //        },
            //        Tipo = TransactionTypes.UsedService,
            //        Valor = Pricing.ToNegative(data.Valor)
            //    };


            //    // Transacción
            //    var responseTransaction = await Transactions.Generate(transaction, context, false);

            //    if (responseTransaction.Response != Responses.Success)
            //    {
            //        dbTransaction.Rollback();
            //        return new();
            //    }

            //    data.Transaction.ID = responseTransaction.LastID;

            //    // Agrega el uso
            //    await context.DataBase.ApiKeyUses.AddAsync(data);

            //    // Guarda los cambios
            //    context.DataBase.SaveChanges();

            //    // Envía la transacción
            //    dbTransaction.Commit();

            //    // Cierra la conexión
            //    return new(Responses.Success, data.ID);

            //}
            //catch (Exception ex)
            //{
            //    dbTransaction.Rollback();
            //    if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert explicit value for identity"))
            //    {

            //    }

            //    ServerLogger.LogError("AJ" + ex.InnerException);
            //}
        }

        return new();
    }



    /// <summary>
    /// Genera un uso en una api key
    /// </summary>
    /// <param name="data">Modelo del uso</param>
    /// <param name="key">String de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> GenerateUses(BillingItemModel data, string key, Conexión context)
    {

        // Ejecución
        try
        {

            //// Obtiene la llave
            //var apiKey = await Query.ApiKeys.ReadBy(key, context).Select(T => T.ID).FirstOrDefaultAsync();

            //// No existe
            //if (apiKey <= 0)
            //{
            //    return new(Responses.InvalidApiKey);
            //}

            //data.Key.ID = apiKey;
            //return await GenerateUses(data, context);
        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new();
    }


}