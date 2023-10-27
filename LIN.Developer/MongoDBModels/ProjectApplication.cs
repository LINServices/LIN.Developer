using LIN.Types.Developer.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LIN.Developer.MongoDBModels;


public class ProjectApplication : IProjectApplication
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; }


    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("key")]
    public string AppKey { get; set; }


}
