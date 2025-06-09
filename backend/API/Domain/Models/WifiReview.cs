using System.Text.Json.Serialization;
using DnsClient;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class WifiReview : TimeStampedEntity
{
    private string _wifiId;

    [BsonElement("WifiNetworkID")]
    [JsonPropertyName("WifiNetworkID")]
    public string WifiId
    {
        get => _wifiId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"WifiId cannot be set to {value}");
            } 
            _wifiId = value;
        }
    }

    private string _userId;
    [BsonElement("UserID")]
    [JsonPropertyName("UserID")]
    public string UserId
    {
        get =>_userId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"UserId cannot be set to {value}");
            }
            _userId = value;
        }
    }

    private string? _text;

    [BsonElement("Comment")]
    [JsonPropertyName("Comment")]
    public string? Text
    {
        get => _text;
        set
        {
            if (!string.IsNullOrEmpty(value) && (value.Length < 5 || value.Length > 100))
            {
                throw new ArgumentException("Text must be between 5 and 100 characters long");
            }
            _text = value;
        }
    }

    private int _rating;

    [BsonElement("Rating")]
    public int Rating
    {
        get => _rating;
        set
        {
            if (value < 1 || value > 10)
                throw new ArgumentException("Rating must be between 1 and 10");
            _rating = value;
        }
    }
    
    // need to add null checks for the properties below
    // if property is null, throw ArgumentNullException
    
    public string City { get; set; }
    
    public string Street { get; set; }
    
    public int BuildingNumber { get; set; }
}