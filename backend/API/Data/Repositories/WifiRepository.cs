using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class WifiRepository : BaseRepository<Wifi>
{
    public WifiRepository(IMongoDatabase database) : base (database, "Favorite-WiFi-Spots")
    {
    }

    public async Task<List<Wifi>> GetByCityAsync (string city)
    {
        var filter = Builders<Wifi>.Filter.Eq(w => w.City, city);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<Wifi?> GetByIdAsync (string id)
    {
        var filter = Builders<Wifi>.Filter.Eq(w => w.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}