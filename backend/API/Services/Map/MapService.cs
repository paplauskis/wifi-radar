using API.Domain.Models;
using API.Services.Interfaces.Map;

namespace API.Services.Map;

public class MapService : IMapService
{
    public async Task<List<WifiNetwork>> Search(string city, int? radius)
    {
        throw new NotImplementedException();
    }
}