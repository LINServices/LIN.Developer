using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LIN.Developer.MongoDBModels;

public class KeyModel : ApiKeyDataModel
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public new string ID { get; set; }


}
