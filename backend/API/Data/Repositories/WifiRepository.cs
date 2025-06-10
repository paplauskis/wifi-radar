using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class WifiRepository : BaseRepository<WifiNetwork>, IWifiRepository
{
    public WifiRepository(IMongoDatabase database) : base(database, "Favorite-WiFi-Spots")
    {
    }

    public async Task<List<WifiNetwork>> GetByCityAsync(string city)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.City, city);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task UpdatePasswordAsync(string wifiId, string password)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifiId);
        var update = Builders<WifiNetwork>.Update.Set(w => w.Password, password);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task AddPasswordAsync(string city, string street, int? buildingNumber, string password)
    {
        var filter = Builders<WifiNetwork>.Filter.And(
            Builders<WifiNetwork>.Filter.Eq(w => w.City, city),
            Builders<WifiNetwork>.Filter.Eq(w => w.Street, street),
            Builders<WifiNetwork>.Filter.Eq(w => w.BuildingNumber, buildingNumber)
        );
        
        var update = Builders<WifiNetwork>.Update.Push(w => w.Passwords, password);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<object>> GetPasswordsByWifiIdAsync(string wifiId)
    {
        var filter = Builders<WifiNetwork>.Filter.Eq(w => w.Id, wifiId);
        var projection = Builders<WifiNetwork>.Projection.Include(w => w.Passwords).Exclude("_id");

        var result = await _collection.Find(filter).Project<WifiNetwork>(projection).FirstOrDefaultAsync();
        return result?.Passwords ?? new List<string>();
    }

    public async Task<List<string>> GetPasswordsByWifiAddressAsync(string city, string street, int buildingNumber)
    {
        throw new NotImplementedException();
    }
}
