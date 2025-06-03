using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository : ICrudRepository<WifiNetwork>
{
    Task AddPasswordAsync(string wifiId, string password);
    Task<List<WifiNetwork>> GetByCityAsync(string city);
    Task<IEnumerable<object>> GetPasswordsByWifiIdAsync(string wifiId);
    Task UpdatePasswordAsync(string wifiId, string password);
}