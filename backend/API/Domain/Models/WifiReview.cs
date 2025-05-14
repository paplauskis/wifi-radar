using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class WifiReview : TimeStampedEntity
{
    [BsonElement("WifiNetworkID")]
    public string WifiId { get; set; }

    [BsonElement("UserID")]
    public string UserId { get; set; }

    [BsonElement("Comment")]
    public string? Text { get; set; }

    [BsonElement("Rating")]
    public int Rating { get; set;}
}