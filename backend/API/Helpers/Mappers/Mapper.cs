using API.Domain.Dto;
using API.Domain.Models;

namespace API.Helpers.Mappers;

public class Mapper
{
    public static WifiNetwork MapDtoToWifiNetwork(WifiNetworkDto dto)
    {
        return new WifiNetwork
        {
            BuildingNumber = (int)dto.BuildingNumber, 
            PlaceName = dto.Name, 
            City = dto.City, 
            Street = dto.Street,
            IsFree = dto.IsFree, 
            UserId = dto.UserId,
            Password = dto.Password,
            PostalCode = dto.PostalCode,
            Id = dto.WifiId
        };
    }
    
    public static WifiNetworkDto MapWifiNetworkToDto(WifiNetwork dto)
    {
         return new WifiNetworkDto
        {
            BuildingNumber = dto.BuildingNumber, 
            Name = dto.PlaceName, 
            City = dto.City, 
            Street = dto.Street,
            IsFree = dto.IsFree, 
            UserId = dto.UserId, 
            WifiId = dto.Id, 
            PostalCode = dto.PostalCode, 
            Password = dto.Password
        };
    }
}