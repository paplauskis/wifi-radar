using API.Data.Repositories.Interfaces;
using API.Domain;
using API.Services.Interfaces.Wifi;

namespace API.Services.Wifi;

public class WifiPasswordSharingService : IWifiPasswordSharingService
{
    private readonly IWifiRepository _wifiRepository;

    public WifiPasswordSharingService(IWifiRepository wifiRepository)
    {
        _wifiRepository = wifiRepository;
    }

    public async Task<string> AddPasswordAsync(PasswordDto passwordDto)
    {
        if (passwordDto == null)
            throw new ArgumentNullException(nameof(passwordDto));

        if (string.IsNullOrWhiteSpace(passwordDto.WifiId))
            throw new ArgumentException("Wifi ID is required.", nameof(passwordDto.WifiId));

        if (string.IsNullOrWhiteSpace(passwordDto.Password))
            throw new ArgumentException("Password cannot be empty.", nameof(passwordDto.Password));

        var wifi = await _wifiRepository.GetByIdAsync(passwordDto.WifiId);
        if (wifi == null)
            throw new KeyNotFoundException($"Wifi network with ID '{passwordDto.WifiId}' not found.");

        await _wifiRepository.AddPasswordAsync(passwordDto.WifiId, passwordDto.Password);
        return passwordDto.Password;
    }

    public async Task<List<string>> GetPasswordsAsync(string wifiId)
    {
        if (string.IsNullOrWhiteSpace(wifiId))
            throw new ArgumentException("Wifi ID is required.", nameof(wifiId));

        var wifi = await _wifiRepository.GetByIdAsync(wifiId);
        if (wifi == null)
            throw new KeyNotFoundException($"Wifi network with ID '{wifiId}' not found.");

        var passwords = await _wifiRepository.GetPasswordsByWifiIdAsync(wifiId);
        return (List<string>)passwords;
    }

    public async Task SharePasswordAsync(string wifiNetworkId, string password)
    {
        if (string.IsNullOrWhiteSpace(wifiNetworkId))
            throw new ArgumentException("Wifi network ID cannot be empty.", nameof(wifiNetworkId));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        await _wifiRepository.UpdatePasswordAsync(wifiNetworkId, password);
    }
}