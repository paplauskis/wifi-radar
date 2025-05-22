using API.Data.Repositories.Interfaces;
using API.Services.Interfaces.Wifi;

namespace API.Services.Wifi;

public class WifiPasswordSharingService : IWifiPasswordSharingService
{
    private readonly IWifiRepository _wifiRepository;

    public WifiPasswordSharingService(IWifiRepository wifiRepository)
    {
        _wifiRepository = wifiRepository;
    }

    public async Task SharePasswordAsync(string wifiNetworkId, string password)
    {
        if (string.IsNullOrWhiteSpace(wifiNetworkId))
            throw new ArgumentException("Wi-Fi network ID cannot be empty.", nameof(wifiNetworkId));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        await _wifiRepository.UpdatePasswordAsync(wifiNetworkId, password);
    }
}