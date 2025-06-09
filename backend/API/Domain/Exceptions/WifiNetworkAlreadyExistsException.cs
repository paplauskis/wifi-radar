using API.Domain.Dto;
using API.Domain.Models;

namespace API.Domain.Exceptions;

public class WifiNetworkAlreadyExistsException : Exception
{
    WifiNetworkDto WifiNetwork { get; init; }
    
    public WifiNetworkAlreadyExistsException(string message, WifiNetworkDto dto) : base(message)
    {
        WifiNetwork = dto;
    }
}