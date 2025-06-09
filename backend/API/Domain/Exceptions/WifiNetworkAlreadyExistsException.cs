using API.Domain.Dto;
using API.Domain.Models;

namespace API.Domain.Exceptions;

public class WifiNetworkAlreadyExistsException : Exception
{
    public WifiNetworkDto WifiNetwork { get; private set; }
    
    public WifiNetworkAlreadyExistsException(string message, WifiNetworkDto dto) : base(message)
    {
        WifiNetwork = dto;
    }
}