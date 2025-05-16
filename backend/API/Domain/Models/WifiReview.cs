using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace API.Domain.Models;

public class WifiReview : TimeStampedEntity
{
    public WifiReview() { }

    [SetsRequiredMembers]
    public WifiReview(string comment, int rating, long userID, string wifiId, DateTime createdAt)
    {
        Comment = comment;
        Rating = rating;
        UserID = userID;
        WifiId = wifiId;
        CreatedAt = createdAt;

        Validate();
    }

    [BsonElement("Comment")]
    [JsonPropertyName("WifiNetworkID")]
    public required string Comment { get; init; }

    [BsonElement("Rating")]
    public required int Rating { get; init; }

    [BsonElement("UserID")]
    [JsonPropertyName("UserID")]
    public required long UserID { get; init; }

    [BsonElement("WifiNetworkID")]
    [JsonPropertyName("WifiNetworkID")]
    public required string WifiId { get; init; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Comment) || Comment.Length > 30)
            throw new ArgumentException("Comment must be between 1 and 30 characters.");

        if (Rating < 1 || Rating > 10)
            throw new ArgumentException("Rating must be between 1 and 10.");

        if (UserID <= 0)
            throw new ArgumentException("UserID must be a positive number.");

        if (string.IsNullOrWhiteSpace(WifiId))
            throw new ArgumentException("WifiId is required.");

        if (CreatedAt == default)
            throw new ArgumentException("CreatedAt date is required.");
    }
}
