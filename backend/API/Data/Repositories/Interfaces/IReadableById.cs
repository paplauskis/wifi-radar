using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IReadableById<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(string id);
}