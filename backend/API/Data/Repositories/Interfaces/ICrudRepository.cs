using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface ICrudRepository<T> :
    IReadableById<T>,
    ICreatable<T>,
    IDeletable<T>,
    IUpdatable<T>
    where T : BaseEntity
{
}