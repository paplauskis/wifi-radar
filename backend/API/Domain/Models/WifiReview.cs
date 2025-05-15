using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class WifiReview : TimeStampedEntity
{
    [BsonElement("WifiNetworkID")]
    [JsonPropertyName("WifiNetworkID")]
    public string WifiId { get; set; }

    [BsonElement("UserID")]
    [JsonPropertyName("UserID")]
    public string UserId { get; set; }

    [BsonElement("Comment")]
    [JsonPropertyName("Comment")]
    public string? Text { get; set; }

    [BsonElement("Rating")]
    public int Rating { get; set;}
}