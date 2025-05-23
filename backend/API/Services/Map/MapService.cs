using API.Domain.Dto;
using API.Domain.Models;
using API.Services.Interfaces.Map;

namespace API.Services.Map;

public class MapService : IMapService
{
    public async Task<List<WifiNetworkDto>> Search(string city, int? radius)
    {
        if (string.IsNullOrEmpty(city)) 
            throw new ArgumentNullException(nameof(city), "City cannot be null or empty");

        List<WifiNetworkDto> wifis;
        
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
        
        return wifis;
    }

    private async Task<List<WifiNetworkDto>> SearchInCity(string city)
    {
        throw new NotImplementedException();
    }

    private async Task<List<WifiNetworkDto>> SearchInRadius(string city, int radius)
    {
        throw new NotImplementedException();
    }
}