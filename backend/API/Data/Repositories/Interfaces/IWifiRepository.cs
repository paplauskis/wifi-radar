using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository : ICrudRepository<WifiNetwork>
{
    Task AddPasswordAsync(string city, string street, int? buildingNumber, string password);
    Task<List<WifiNetwork>> GetByCityAsync(string city);
    Task<IEnumerable<object>> GetPasswordsByWifiIdAsync(string wifiId);
    Task UpdatePasswordAsync(string wifiId, string password);
    Task<List<string>> GetPasswordsByWifiAddressAsync(string city, string street, int buildingNumber);
}