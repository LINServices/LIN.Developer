using LIN.Developer.Data.Mongo;
using LIN.Types.Developer.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LIN.Developer.Services;


public class MongoService
{


    /// <summary>
    /// Cadena de conexión.
    /// </summary>
    private static string ConnectionString { get; set; } = string.Empty;



    /// <summary>
    /// Cliente de Mongo.
    /// </summary>
    public MongoClient MongoClient { get; set; }



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




    public async Task<(string id, bool success)> Insert<T>(T element, string collectionName) where T : IMongoBase
    {

        try
        {
            var client = MongoClient;
            var db = client.GetDatabase("Cluster0");


            var collection = db.GetCollection<T>(collectionName);

            await collection.InsertOneAsync(element);

            return (element.Id.ToString(), true);
        }
        catch
        {

        }

        return ("", false);

    }



    public static MongoService GetOneConnection()
    {
        return new();
    }

}
