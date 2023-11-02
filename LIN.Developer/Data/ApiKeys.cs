using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Data;


public static class ApiKeys
{


    #region Abstracciones


    /// <summary>
    /// Crea una Api Key en la base de datos
    /// </summary>
    /// <param name="data">Modelo de la llave</param>
    public async static Task<CreateResponse> Create(KeyModel data)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await Create(data, context);
        context.CloseActions(connectionKey);
        return response;

    }



    /// <summary>
    /// Obtiene las llaves asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    public async static Task<ReadAllResponse<KeyModel>> ReadAll(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Cambia el estado de una Key
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="estado">Nuevo estado</param>
    public async static Task<ResponseBase> UpdateState(int key, ApiKeyStatus estado)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var response = await UpdateState(key, estado, context);
        context.CloseActions(connectionKey);
        return response;

    }



    /// <summary>
    /// Obtiene una api key
    /// </summary>
    /// <param name="key">String de la llave</param>
    public async static Task<ReadOneResponse<KeyModel>> ReadBy(string key)
    {
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await ReadBy(key, context);
        context.CloseActions(connectionKey);
        return response;
    }



    /// <summary>
    /// Obtiene una api key
    /// </summary>
    /// <param name="key">ID de la llave</param>
    public async static Task<ReadOneResponse<KeyModel>> ReadBy(int key)
    {
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();
        var response = await ReadBy(key, context);
        context.CloseActions(connectionKey);
        return response;
    }


    #endregion



    /// <summary>
    /// Crea una nueva key
    /// </summary>
    /// <param name="data">Modelo del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(KeyModel data, Conexión context)
    {

        data.ID = 0;

        // Ejecución
        try
        {
            var res = await context.DataBase.Keys.AddAsync(data);
            context.DataBase.SaveChanges();
            return new(Responses.Success, data.ID);
        }
        catch (Exception ex)
        {
            
        }

        return new();
    }



    /// <summary>
    /// Obtiene una api key
    /// </summary>
    /// <param name="key">String de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<KeyModel>> ReadBy(string key, Conexión context)
    {
        // Ejecución
        try
        {

            var res = await Query.ApiKeys.ReadBy(key, context).FirstOrDefaultAsync();

            if (res == null)
            {
                return new(Responses.InvalidApiKey);
            }

            return new(Responses.Success, res);
        }
        catch (Exception ex)
        {
            
        }

        return new();
    }



    /// <summary>
    /// Obtiene una api key
    /// </summary>
    /// <param name="id">ID de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<KeyModel>> ReadBy(int id, Conexión context)
    {
        // Ejecución
        try
        {
            var res = await Query.ApiKeys.ReadBy(id, context).FirstOrDefaultAsync();

            if (res == null)
            {
                return new(Responses.InvalidApiKey);
            }

            return new(Responses.Success, res);
        }
        catch
        {
            
        }

        return new();
    }



    /// <summary>
    /// Obtiene las llaves asociadas a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<KeyModel>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {
            // retorna la lista
            var lista = await Query.ApiKeys.ReadAll(id, context).ToListAsync();

            return new(Responses.Success, lista);
        }
        catch
        {
        }

        return new();
    }



    /// <summary>
    /// Cambia el estado de una llave
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="estado">Nuevo estado de la llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ResponseBase> UpdateState(int key, ApiKeyStatus estado, Conexión context)
    {
        // Ejecución
        try
        {
            var modelo = await context.DataBase.Keys.FindAsync(key);

            if (modelo == null)
            {
                return new(Responses.NotRows);
            }

            modelo.Status = estado;
            context.DataBase.SaveChanges();
            return new(Responses.Success);
        }
        catch (Exception ex)
        {
            
        }

        return new();
    }



    /// <summary>
    /// Obtiene el ID del proyecto al cual esta anclado una llave
    /// </summary>
    /// <param name="key">llave</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<int>> GetProjectID(string key, Conexión context)
    {
        // Ejecución
        try
        {

            var id = await Query.ApiKeys.GetProjectID(key, context).FirstOrDefaultAsync();


            // Si es un ID invalido
            if (id <= 0)
                return new(Responses.NotRows, 0)
                {
                    Message = $"La llave '{key}' es invalida o esta desactivada."
                };

            // Correcto
            return new(Responses.Success, id);

        }
        catch (Exception ex)
        {
            
        }

        return new();
    }



}