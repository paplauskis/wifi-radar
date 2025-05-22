using API.Domain.Models;

namespace API.Services.Interfaces.Wifi;

public interface IWifiSearchService
{
    Task<List<WifiNetwork>> GetWifi(string city);
}