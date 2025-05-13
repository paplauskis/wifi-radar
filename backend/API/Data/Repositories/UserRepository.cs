using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase database) : base(database, "Users")
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

}