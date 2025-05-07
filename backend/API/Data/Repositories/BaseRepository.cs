using API.Data.Repositories.Interfaces;
using API.Domain.Models;

namespace API.Data.Repositories;

public abstract class BaseRepository<T> : ICrudRepository<T> where T : BaseEntity
{
    public async Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<T> AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(T entityToBeDeleted)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entityToBeUpdated)
    {
        throw new NotImplementedException();
    }
}