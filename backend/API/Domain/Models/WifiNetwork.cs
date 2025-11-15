using System.Security.Cryptography;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Domain.Models;

public class WifiNetwork : TimeStampedEntity
{
    private string _userId;

    [BsonElement("UserID")]
    [JsonPropertyName("UserID")]
    public string UserId
    {
        get => _userId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"UserId cannot be set to {value}");
            _userId = value;
        }
    }

    private string _city;

    [BsonElement("City")]
    public string City
    {
        get => _city;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"City cannot be set to {value}");
            _city = value;
        }
    }

    private string _placeName;

    [BsonElement("SpotName")]
    [JsonPropertyName("SpotName")]
    public string PlaceName
    {
        get => _placeName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"PlaceName cannot be set to {value}");
            _placeName = value;
        }
    }

    private string _street;

    [BsonElement("StreetName")]
    [JsonPropertyName("StreetName")]
    public string Street
    {
        get => _street;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Street cannot be set to {value}");
            _street = value;
        }
    }

    private int _buildingNumber;

    [BsonElement("BuildingNumber")]
    public int BuildingNumber
    {
        get => _buildingNumber;
        set
        {
            if (value <= 0 || value == int.MaxValue)
                throw new ArgumentException($"BuildingNumber cannot be set to {value}");
            _buildingNumber = value;
        }
    }

    private int _postalCode;

    [BsonElement("ZIPCode")]
    [JsonPropertyName("ZIPCode")]
    public int? PostalCode
    {
        get => _postalCode;
        set
        {
            if (value.HasValue)
            {
                if (value <= 0 || value > 99999)
                    throw new ArgumentException($"PostalCode cannot be set to {value}");
                _postalCode = value.Value;
            }
            else
            {
                _postalCode = default;
            }
        }
    }

    private bool _isFree;

    [BsonElement("IsFree")]
    public bool IsFree
    {
        get => _isFree;
        set => _isFree = value;
    }

    private string _password;

    [BsonElement("Password")]
    public string? Password
    {
        get => _password;
        set
        {
            if (!_isFree && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Password cannot be set to {value}");

            if (!string.IsNullOrWhiteSpace(value) && value.Length > 100)
                throw new ArgumentException($"Password cannot be set to {value}");

            _password = value;
        }
    }

    private List<string> _passwords = new();

    [BsonElement("Passwords")]
    [JsonPropertyName("Passwords")]
    public List<string> Passwords
    {
        get => _passwords;
        set => _passwords = value ?? new List<string>();
    }
}
