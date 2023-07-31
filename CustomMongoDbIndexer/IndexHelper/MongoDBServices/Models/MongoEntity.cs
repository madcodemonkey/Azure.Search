using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBServices;

public abstract class MongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
}