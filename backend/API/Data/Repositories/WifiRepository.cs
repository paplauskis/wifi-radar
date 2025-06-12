using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class WifiRepository : IWifiRepository
{
    private readonly IMongoCollection<WifiNetwork> _wifiNetworks;

    public WifiRepository(IMongoDatabase database)
    {
        _wifiNetworks = database.GetCollection<WifiNetwork>("WifiNetworks");
    }

    public async Task<WifiNetwork?> FindById(string id)
    {
        return await _wifiNetworks.Find(w => w.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<WifiNetwork>> Find(FilterDefinition<WifiNetwork> filter)
    {
        return await _wifiNetworks.Find(filter).ToListAsync();
    }

    public async Task InsertOne(WifiNetwork wifi)
    {
        await _wifiNetworks.InsertOneAsync(wifi);
    }

    public async Task UpdateOne(WifiNetwork wifi)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifi.Id);
        await _wifiNetworks.ReplaceOneAsync(filter, wifi);
    }

    public async Task DeleteOne(string id)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, id);
        await _wifiNetworks.DeleteOneAsync(filter);
    }

    public async Task<List<WifiNetwork>> GetByCityAsync(string city)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.City, city);
        return await _wifiNetworks.Find(filter).ToListAsync();
    }

    public async Task UpdatePasswordAsync(string wifiId, string password)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifiId);
        var update = Builders<WifiNetwork>.Update.Set(w => w.Password, password);
        await _wifiNetworks.UpdateOneAsync(filter, update);
    }

    public async Task AddPasswordAsync(string city, string street, int? buildingNumber, string password)
    {
        var filter = Builders<WifiNetwork>.Filter.And(
            Builders<WifiNetwork>.Filter.Eq(w => w.City, city),
            Builders<WifiNetwork>.Filter.Eq(w => w.Street, street),
            Builders<WifiNetwork>.Filter.Eq(w => w.BuildingNumber, buildingNumber)
        );
        
        var update = Builders<WifiNetwork>.Update.Push(w => w.Passwords, password);
        await _wifiNetworks.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<object>> GetPasswordsByWifiIdAsync(string wifiId)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifiId);
        var projection = Builders<WifiNetwork>.Projection.Include(w => w.Passwords).Exclude("_id");

        var result = await _wifiNetworks.Find(filter).Project<WifiNetwork>(projection).FirstOrDefaultAsync();
        return result?.Passwords ?? new List<string>();
    }

    public async Task<List<string>> GetPasswordsByWifiAddressAsync(string city, string street, int buildingNumber)
    {
        var filter = Builders<WifiNetwork>.Filter.And(
            Builders<WifiNetwork>.Filter.Eq(w => w.City, city),
            Builders<WifiNetwork>.Filter.Eq(w => w.Street, street),
            Builders<WifiNetwork>.Filter.Eq(w => w.BuildingNumber, buildingNumber)
            );

        var projection = Builders<WifiNetwork>.Projection.Include(w => w.Passwords).Exclude("_id");

        var result = await _wifiNetworks.Find(filter).Project<WifiNetwork>(projection).FirstOrDefaultAsync();
        return result?.Passwords ?? new List<string>();
    }
}
