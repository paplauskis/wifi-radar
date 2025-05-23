using System.Text.Json.Serialization;

namespace API.Domain.Dto;

public class OverpassResponseElementDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("lat")]
    public decimal? Latitude { get; set; }
    
    [JsonPropertyName("lon")]
    public decimal? Longitude { get; set; }

    [JsonPropertyName("tags")]
    public OverpassElementTags? Tags { get; set; } = new();
}