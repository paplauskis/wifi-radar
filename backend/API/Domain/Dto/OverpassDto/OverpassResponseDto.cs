using System.Text.Json.Serialization;

namespace API.Domain.Dto.OverpassDto;

public class OverpassResponseDto
{
    [JsonPropertyName("version")]
    public decimal? Version { get; set; }

    [JsonPropertyName("elements")]
    public List<OverpassResponseElementDto>? Elements { get; set; }
}