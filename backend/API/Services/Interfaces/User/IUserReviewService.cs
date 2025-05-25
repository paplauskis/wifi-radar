using API.Domain.Models;

namespace API.Services.Interfaces.User
{
    public interface IUserReviewService
    {
        Task AddReviewAsync(WifiReview review);
        Task<List<WifiReview>> GetReviewAsync(string wifiId);
    }
}