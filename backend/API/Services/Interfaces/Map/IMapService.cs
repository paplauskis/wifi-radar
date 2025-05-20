using API.Domain.Models;

namespace API.Services.Interfaces.Map;

public interface IMapService
{ 
    Task<List<WifiNetwork>> Search(string city, int? radius);
}