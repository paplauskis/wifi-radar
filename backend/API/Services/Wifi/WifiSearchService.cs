using API.Data.Repositories.Interfaces;
using API.Domain;
using API.Services.Interfaces.Wifi;
using API.Domain.Models;
using API.Domain.Dto;

namespace API.Services.Wifi;

public class WifiSearchService : IWifiSearchService
{
    private readonly IWifiRepository _wifiRepository;
    private readonly IWifiReviewRepository _wifiReviewRepository;

    public WifiSearchService(IWifiRepository repo, IWifiReviewRepository wifiReviewRepository)
    {
        _wifiRepository = repo;
        _wifiReviewRepository = wifiReviewRepository;
    }

    public Task<List<WifiReview>> GetReviewsAsync(string wifiId)
    {
        throw new NotImplementedException();
    }

    public Task<WifiReview> AddReviewAsync(WifiReviewDto wifiReviewDto)
    {
        throw new NotImplementedException();
    }
}