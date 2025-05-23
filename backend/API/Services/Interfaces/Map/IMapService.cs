using API.Domain.Dto.OverpassDto;

namespace API.Services.Interfaces.Map;

public interface IMapService
{ 
    Task<List<OverpassResponseElementDto>> Search(string city, int? radius);
}