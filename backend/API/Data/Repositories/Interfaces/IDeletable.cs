using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IDeletable<T> where T : BaseEntity
{
    Task DeleteAsync(T entityToBeDeleted);
}