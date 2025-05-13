using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class Wifi : TimeStampedEntity
{
    [BsonElement("UserID")]
    public int UserId { get; set; }

    [BsonElement("Country")]
    public string Country { get; set; }

    [BsonElement("City")]
    public string City { get; set; }

    [BsonElement("SpotName")]
    public string PlaceName { get; set; }

    [BsonElement("StreetName")]
    public string Street { get; set; }

    [BsonElement("BuildingNumber")]
    public int BuildingNumber { get; set; }

    [BsonElement("ZIPCode")]
    public int? PostalCode { get; set; }

    [BsonElement("IsFree")]
    public bool IsFree { get; set; }

    [BsonElement("Password")]
    public string? Password { get; set; }
}