namespace API.Services.Interfaces.Wifi;

public interface IWifiPasswordSharingService
{
    Task SharePasswordAsync(string wifiNetworkId, string password);
}