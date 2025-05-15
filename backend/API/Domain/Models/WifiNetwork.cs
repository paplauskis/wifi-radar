using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class WifiNetwork : TimeStampedEntity
{
    [BsonElement("UserID")]
    [JsonPropertyName("UserID")]
    public string UserId { get; set; }

    [BsonElement("Country")]
    public string Country { get; set; }

    [BsonElement("City")]
    public string City { get; set; }

    [BsonElement("SpotName")]
    [JsonPropertyName("SpotName")]
    public string PlaceName { get; set; }

    [BsonElement("StreetName")]
    [JsonPropertyName("StreetName")]
    public string Street { get; set; }

    [BsonElement("BuildingNumber")]
    public int BuildingNumber { get; set; }

    [BsonElement("ZIPCode")]
    [JsonPropertyName("ZIPCode")]
    public int? PostalCode { get; set; }

    [BsonElement("IsFree")]
    public bool IsFree { get; set; }

    [BsonElement("Password")]
    public string? Password { get; set; }
}