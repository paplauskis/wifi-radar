using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class User : TimeStampedEntity
{
    [BsonElement("Username")]
    public string Username { get; set; }

    [BsonElement("Password")]
    public string Password { get; set; }
}