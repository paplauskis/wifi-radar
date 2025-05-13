using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class WifiReviewRepository : BaseRepository<WifiReview>, IWifiReviewRepository
{
    public WifiReviewRepository(IMongoDatabase database) : base(database, "Reviews")
    {
    }

    public async Task AddReviewAsync(WifiReview review)
    {
        await _collection.InsertOneAsync(review);
    }

    public async Task<List<WifiReview>> GetReviewsByWifiIdAsync(string wifiId)
    {
        var filter = Builders<WifiReview>.Filter.Eq("WifiId", wifiId);
        return await _collection.Find(filter).ToListAsync();
    }
}