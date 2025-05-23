using System.Text.Json.Serialization;

namespace API.Domain.Dto.OverpassDto;

public class OverpassElementTags
{
    [JsonPropertyName("addr:city")]
    public string? City { get; set; }

    [JsonPropertyName("name")]
    public string? PlaceName { get; set; }
    
    [JsonPropertyName("addr:street")]
    public string? Street { get; set; }
    
    [JsonPropertyName("addr:housenumber")]
    public string? BuildingNumber { get; set; }

    [JsonPropertyName("addr:postcode")]
    public string? PostalCode { get; set; }
    
    [JsonPropertyName("internet_access:ssid")]
    public string? WifiName { get; set; }
}