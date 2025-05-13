using API.Data.Repositories.Interfaces;
using API.Services.Interfaces.Wifi;
using API.Domain.Models;

namespace API.Services.Wifi;

public class WifiSearchService : IWifiSearchService
{
    private readonly IWifiRepository _wifiRepository;
    
    public WifiSearchService(IWifiRepository repo)
    {
        _wifiRepository = repo;
    }

    public async Task<List<WifiNetwork>> GetWifi()
    {
        throw new NotImplementedException();
    }
}