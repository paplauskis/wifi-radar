using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace API.Domain.Models;

public class WifiNetwork : TimeStampedEntity
{
    public WifiNetwork() { }

    [SetsRequiredMembers]
    public WifiNetwork(string city, string country, string placeName, string street,
                       int postalCode, int buildingNumber, bool isFree,
                       string? password, long userID)
    {
        City = city;
        Country = country;
        PlaceName = placeName;
        Street = street;
        PostalCode = postalCode;
        BuildingNumber = buildingNumber;
        IsFree = isFree;
        Password = password;
        UserID = userID;

        Validate();
    }

    [BsonElement("City")]
    public required string City { get; set; }

    [BsonElement("Country")]
    public required string Country { get; set; }

    [BsonElement("PlaceName")]
    public required string PlaceName { get; set; }

    [BsonElement("Street")]
    public required string Street { get; set; }

    [BsonElement("PostalCode")]
    public required int PostalCode { get; set; }

    [BsonElement("BuildingNumber")]
    public required int BuildingNumber { get; set; }

    [BsonElement("IsFree")]
    public required bool IsFree { get; set; }

    [BsonElement("Password")]
    public string? Password { get; set; }

    [BsonElement("UserID")]
    public required long UserID { get; set; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(City)) 
            throw new ArgumentException("City is required.");

        if (string.IsNullOrWhiteSpace(Country)) 
            throw new ArgumentException("Country is required.");

        if (string.IsNullOrWhiteSpace(PlaceName)) 
            throw new ArgumentException("Place name is required.");

        if (string.IsNullOrWhiteSpace(Street)) 
            throw new ArgumentException("Street is required.");

        if (PostalCode < 1 || PostalCode > 99999) 
            throw new ArgumentException("Postal code must be 1–99999.");

        if (BuildingNumber < 1) 
            throw new ArgumentException("Building number must be a positive integer.");

        if (UserID <= 0) 
            throw new ArgumentException("UserID must be a positive number.");

        if (!IsFree && string.IsNullOrWhiteSpace(Password)) 
            throw new ArgumentException("Password is required for non-free WiFi.");

        if (Password is not null)
        {
            if (Password.Length > 20) 
                throw new ArgumentException("Password must be 20 characters or less.");

            if (!Password.Any(char.IsUpper)) 
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!Password.Any(char.IsDigit)) 
                throw new ArgumentException("Password must contain at least one digit.");

            if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new ArgumentException("Password must contain at least one special character.");
        }
    }
}
