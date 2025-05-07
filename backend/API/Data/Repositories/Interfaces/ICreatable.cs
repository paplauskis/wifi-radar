using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface ICreatable<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity);
}