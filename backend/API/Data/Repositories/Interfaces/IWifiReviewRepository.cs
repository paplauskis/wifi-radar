using API.Domain.Models;

namespace API.Data.Repositories.Interfaces;

public interface IWifiReviewRepository
{
    Task AddReviewAsync(WifiReview review);

    Task<List<WifiReview>> GetReviewsByWifiIdAsync(string wifiId);
}