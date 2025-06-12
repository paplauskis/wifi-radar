using API.Domain.Models;
using MongoDB.Driver;

namespace API.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> FindById(string id);
    Task<List<User>> Find(FilterDefinition<User> filter);
    Task InsertOne(User user);
    Task UpdateOne(User user);
    Task DeleteOne(string id);
    
    // Additional methods needed by services
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}