using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
    }

    public async Task<User?> FindById(string id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<User>> Find(FilterDefinition<User> filter)
    {
        return await _users.Find(filter).ToListAsync();
    }

    public async Task InsertOne(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateOne(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        await _users.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteOne(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        await _users.DeleteOneAsync(filter);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        return await _users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }
}