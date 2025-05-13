using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiRepository
{
    Task<List<Wifi>> GetByCityAsync(string city);
}