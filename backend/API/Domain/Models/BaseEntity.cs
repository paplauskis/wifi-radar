using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API.Domain.Models;

public abstract class BaseEntity
{
    private string? _id;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id
    {
        get => _id;
        set
        {
            if (value is not null && string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Id cannot be set to {value}");
            }
            _id = value;
        }
    }
}