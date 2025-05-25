using API.Domain;
using API.Domain.Dto;
using API.Domain.Models;

namespace API.Services.Interfaces.Wifi;

public interface IWifiSearchService
{
    Task<List<WifiReview>> GetReviewsAsync(string wifiId);
    Task<WifiReview> AddReviewAsync(WifiReviewDto wifiReviewDto);
}