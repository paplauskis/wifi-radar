using API.Domain.Models;
using API.Services.Interfaces.User;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Users;

public class UserReviewService : IUserReviewService
{
    private readonly IMongoCollection<WifiReview> _reviews;

    public UserReviewService(IMongoDatabase database)
    {
        _reviews = database.GetCollection<WifiReview>("Reviews");
    }

    public async Task AddReviewAsync(WifiReview review)
    {
        if (review == null)
            throw new ArgumentNullException(nameof(review));

        await _reviews.InsertOneAsync(review);
    }

    public async Task<List<WifiReview>> GetReviewsByWifiIdAsync(string wifiId)
    {
        if (string.IsNullOrWhiteSpace(wifiId))
            throw new ArgumentException("WifiId is required", nameof(wifiId));

        var filter = Builders<WifiReview>.Filter.Eq(r => r.WifiId, wifiId);
        return await _reviews.Find(filter).ToListAsync();
    }
}