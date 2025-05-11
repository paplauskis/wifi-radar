using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API.Domain.Models;

public abstract class BaseEntity
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}