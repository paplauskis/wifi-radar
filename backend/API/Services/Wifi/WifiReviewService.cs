using API.Data.Repositories.Interfaces;
using API.Domain;
using API.Services.Interfaces.Wifi;
using API.Domain.Models;
using API.Data.Repositories;

namespace API.Services.Wifi;

public class WifiReviewService : IWifiReviewService
{
    private readonly IWifiRepository _wifiRepository;
    private readonly IWifiReviewRepository _wifiReviewRepository;

    public WifiReviewService(IWifiRepository repo, IWifiReviewRepository wifiReviewRepository)
    {
        _wifiRepository = repo;
        _wifiReviewRepository = wifiReviewRepository;
    }

    // should return WifiNetwork reviews that match the city, street and building number.
    // the method currently does not compile, so it needs fixing.
    // method should also check city, street and building number to not be null values,
    // because otherwise the search will not be accurate.
    // if arguments are null, throw ArgumentNullException.
    public async Task<List<WifiReview>> GetReviewsAsync(string city, string street, int buildingNumber)
    {
        if (string.IsNullOrWhiteSpace(wifiId))
            throw new ArgumentException("Wifi ID must be provided.", nameof(wifiId));
        
        var wifi = await _wifiRepository.GetByIdAsync(wifiId);
        
        if (wifi == null)
            throw new KeyNotFoundException($"Wifi network with ID '{wifiId}' was not found.");
        
        return await _wifiReviewRepository.GetReviewsByWifiIdAsync(wifiId);
    }

    public async Task<WifiReview> AddReviewAsync(WifiReviewDto wifiReviewDto)
    {
        if (wifiReviewDto == null)
        {
            throw new ArgumentNullException(nameof(wifiReviewDto));
        }

        var newReview = new WifiReview
        {
            Id = Guid.NewGuid().ToString(),
            UserId = wifiReviewDto.UserId,
            Rating = (int)wifiReviewDto.Rating,
            Text = wifiReviewDto.Text,
            City = wifiReviewDto.City,
            Street = wifiReviewDto.Street,
            BuildingNumber = (int)wifiReviewDto.BuildingNumber,
            CreatedAt = DateTime.UtcNow
        };

        await _wifiReviewRepository.AddReviewAsync(newReview);
        return newReview;
    }
}
