using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IUpdatable<T> where T : BaseEntity
{
    Task<T> UpdateAsync(T entityToBeUpdated);
}