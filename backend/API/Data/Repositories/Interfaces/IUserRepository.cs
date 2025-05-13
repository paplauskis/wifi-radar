using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}