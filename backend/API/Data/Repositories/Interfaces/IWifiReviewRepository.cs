using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiReviewRepository : ICrudRepository<WifiReview>
{
    Task AddReviewAsync(WifiReview review);

    Task<List<WifiReview>> GetReviewsByWifiIdAsync(string wifiId);
}