using LIN.Types.Developer.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LIN.Developer.MongoDBModels;


public class ProjectDB : IProjectDB
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; }


    [BsonElement("connectionString")]
    public string ConnectionString { get; set; }
    public string Name { get ; set ; }
}
