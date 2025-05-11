using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public abstract class TimeStampedEntity : BaseEntity
{
    [BsonElement("DateCreated")]
    public DateTime CreatedAt { get; init; }

    protected TimeStampedEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}