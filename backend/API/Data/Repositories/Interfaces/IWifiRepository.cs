using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository : ICrudRepository<WifiNetwork>
{
    Task<List<WifiNetwork>> GetByCityAsync(string city);
}