using API.Domain;
using API.Services.Interfaces.Wifi;

namespace API.Services.Wifi;

public class WifiPasswordSharingService : IWifiPasswordSharingService
{
    public WifiPasswordSharingService()
    {
        
    }
    
    public Task<string> AddPasswordAsync(PasswordDto passwordDto)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetPasswordsAsync(string wifiId)
    {
        throw new NotImplementedException();
    }
}