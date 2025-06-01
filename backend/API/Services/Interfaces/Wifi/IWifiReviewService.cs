using API.Domain;
using API.Domain.Models;

namespace API.Services.Interfaces.Wifi;

public interface IWifiReviewService
{
    Task<List<WifiReview>> GetReviewsAsync(string wifiId);
    Task<WifiReview> AddReviewAsync(WifiReviewDto wifiReviewDto);
}