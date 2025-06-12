using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository
{
    Task<WifiNetwork?> FindById(string id);
    Task<List<WifiNetwork>> Find(FilterDefinition<WifiNetwork> filter);
    Task InsertOne(WifiNetwork wifi);
    Task UpdateOne(WifiNetwork wifi);
    Task DeleteOne(string id);
    
    // Additional methods needed by services
    Task<List<WifiNetwork>> GetByCityAsync(string city);
    Task UpdatePasswordAsync(string wifiId, string password);
    Task AddPasswordAsync(string city, string street, int? buildingNumber, string password);
    Task<IEnumerable<object>> GetPasswordsByWifiIdAsync(string wifiId);
    Task<List<string>> GetPasswordsByWifiAddressAsync(string city, string street, int buildingNumber);
}