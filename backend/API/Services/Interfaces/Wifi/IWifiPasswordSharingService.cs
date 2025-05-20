using API.Domain;

namespace API.Services.Interfaces.Wifi;

public interface IWifiPasswordSharingService
{
    Task<string> AddPasswordAsync(PasswordDto passwordDto);
    Task<List<string>> GetPasswordsAsync(string wifiId);
}