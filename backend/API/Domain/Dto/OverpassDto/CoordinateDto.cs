using System.Text.Json.Serialization;

namespace API.Domain.Dto.OverpassDto;

public class CoordinateDto
{
    [JsonPropertyName("lat")]
    public string? Latitude { get; set; }
    
    [JsonPropertyName("lon")]
    public string? Longitude { get; set; }
}