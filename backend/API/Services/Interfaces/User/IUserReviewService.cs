using API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces.User
{
    public interface IUserReviewService
    {
        Task AddReviewAsync(WifiReview review);
        Task<List<WifiReview>> GetReviewsByWifiIdAsync(string wifiId);
    }
}