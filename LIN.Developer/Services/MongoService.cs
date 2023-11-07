using LIN.Developer.Data.Mongo;
using LIN.Types.Developer.Interfaces;

namespace LIN.Developer.Services;


public class MongoService
{


    /// <summary>
    /// Cadena de conexión.
    /// </summary>
    private static string ConnectionString { get; set; } = string.Empty;


    /// <summary>
    /// Nombre de la base de datos.
    /// </summary>
    private static string DataBaseName { get; set; } = "Cluster0";


    /// <summary>
    /// Cliente de Mongo.
    /// </summary>
    private MongoClient MongoClient { get; set; }



    /// <summary>
    /// Base de datos de MongoDB.
    /// </summary>
    public IMongoDatabase DataBase => MongoClient.GetDatabase(DataBaseName);



    /// <summary>
    /// Contexto.
    /// </summary>
    public MongoContext Context { get; set; }




    /// <summary>
    /// Nuevo MongoDB.
    /// </summary>
    public MongoService()
    {
        MongoClient = new MongoClient(ConnectionString);
        Context = MongoContext.Create(MongoClient.GetDatabase("Cluster0"));
    }



    /// <summary>
    /// Establece la cadena de conexión.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión.</param>
    public static void SetConnection(string connectionString) => ConnectionString = connectionString;



    /// <summary>
    /// Insertar un elemento en una colección.
    /// </summary>
    /// <typeparam name="T">Elemento.</typeparam>
    /// <param name="element">Elemento.</param>
    /// <param name="collectionName">Nombre de la colección.</param>
    public async Task<(string id, bool success)> Insert<T>(T element, string collectionName) where T : IMongoBase
    {
        try
        {
            // Obtener la colección.
            var collection = DataBase.GetCollection<T>(collectionName);

            // Agregar el elemento,
            await collection.InsertOneAsync(element);

            // Retornar el Id.
            return (element.Id.ToString(), true);
        }
        catch
        {
        }
        return ("", false);
    }



    /// <summary>
    /// Obtener una conexión con MongoDB.
    /// </summary>
    public static MongoService GetOneConnection()
    {
        return new();
    }


}