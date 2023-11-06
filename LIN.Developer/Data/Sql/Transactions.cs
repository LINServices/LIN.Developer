namespace LIN.Developer.Data.Sql;


public class Transactions
{



    #region Abstracciones



    /// <summary>
    /// Genera una transacción
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Generate(TransactionDataModel data)
    {

        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Generate(data, context, true);

        context.CloseActions(connectionKey);

        return response;

    }



    /// <summary>
    /// Obtiene la lista de transacciones de un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    public async static Task<ReadAllResponse<TransactionDataModel>> ReadAll(int id)
    {

        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;

    }



    #endregion



    /// <summary>
    /// Genera una nueva transacción
    /// </summary>
    /// <param name="data">Modelo</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Generate(TransactionDataModel data, Conexión context, bool confirm)
    {

        // Modelo
        data.ID = 0;

        // Ejecución (Transacción)
        var transaction = context.GetTransaction();

        try
        {



            // Obtiene el perfil
            var profile = await context.DataBase.Profiles.FindAsync(data.Profile.ID);

            // Si no existe
            if (profile == null)
            {
                if (confirm) transaction.Rollback();
                return new(Responses.NotExistProfile);
            }

            // Agrega la transacción
            context.DataBase.Transactions.Add(data);

            // Guarda el origen
            context.DataBase.SaveChanges();

            // Agrega los valores 
            profile.Credits += data.Valor;

            // Guarda los cambios
            context.DataBase.SaveChanges();

            // Envía la transacción
            if (confirm) transaction.Commit();

            // Cierra la conexión
            return new(Responses.Success, data.ID);

        }
        catch (Exception ex)
        {
            if (confirm) transaction.Rollback();

            if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert explicit value for identity"))
            {
            }
        }


        return new();
    }



    /// <summary>
    /// Obtiene la lista de transacciones de origen de un perfil
    /// </summary>
    /// <param name="id">ID de un perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<TransactionDataModel>> ReadAll(int id, Conexión context)
    {
        try
        {

            // Consulta
            var query = Query.Transactions.ReadAll(id, context);

            // Lista de transacciones de origen
            var transacciones = await query.Take(100).ToListAsync();

            return new(Responses.Success, transacciones);

        }
        catch
        {
        }

        return new();
    }



}