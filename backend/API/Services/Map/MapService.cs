using System.Text;
using System.Text.Json;
using API.Domain.Dto.OverpassDto;
using API.Domain.Exceptions;
using API.Helpers;
using API.Services.Interfaces.Map;

namespace API.Services.Map;

public class MapService : IMapService
{
    private readonly HttpClient _client = new();
    
    public async Task<List<OverpassResponseElementDto>> Search(string city, int? radius)
    {
        if (string.IsNullOrEmpty(city)) 
            throw new ArgumentNullException(nameof(city), "City cannot be null or empty");

        List<OverpassResponseElementDto> wifis;
        
        if (radius == null)
        {
            wifis = await SearchInCity(city);
        }
        else
        {
            if (radius < 0) 
                throw new ArgumentException($"Radius cannot be negative; selected radius is {radius} meters");
            if (radius > 100000)
                throw new ArgumentException($"Radius cannot be greater than 100 kilometers; selected radius is {radius / 100} kilometers"); 
            
            wifis = await SearchInRadius(city, (int)radius);
        }

        if (wifis.Count == 0)
            throw new EmptyResponseException("No wifi networks were found in the selected location.");
        
        return wifis;
    }

    private async Task<List<OverpassResponseElementDto>> SearchInCity(string city)
    {
        List<OverpassResponseElementDto> wifis = new();
        var content = new StringContent(
            OverpassApi.FreeWifiInCity(city), 
            Encoding.UTF8, 
            "application/json");

        var deserializedObj = await GetResponseFromApi(content);

        if (deserializedObj != null && deserializedObj.Elements != null)
        {
            foreach (var elem in deserializedObj.Elements)
            {
                wifis.Add(elem);
            }
        }
        
        return wifis;
    }

    private async Task<List<OverpassResponseElementDto>> SearchInRadius(string city, int radius)
    {
        throw new NotImplementedException();
    }

    private async Task<OverpassResponseDto?> GetResponseFromApi(StringContent content)
    {
        var response = await _client.PostAsync(OverpassApi.ApiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<OverpassResponseDto>(responseString);
    }
}