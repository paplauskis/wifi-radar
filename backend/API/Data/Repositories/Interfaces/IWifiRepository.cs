using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository : ICrudRepository<Wifi>
{
    Task<List<Wifi>> GetByCityAsync(string city);
}