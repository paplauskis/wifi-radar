using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class WifiRepository : BaseRepository<WifiNetwork>, IWifiRepository
{
    public WifiRepository(IMongoDatabase database) : base (database, "Favorite-WiFi-Spots")
    {
    }

    public async Task<List<WifiNetwork>> GetByCityAsync (string city)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.City, city);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task UpdatePasswordAsync(string wifiNetworkId, string password)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifiNetworkId);
        var update = Builders<WifiNetwork>.Update.Set(w => w.Password, password);
        await _collection.UpdateOneAsync(filter, update);
    }
}