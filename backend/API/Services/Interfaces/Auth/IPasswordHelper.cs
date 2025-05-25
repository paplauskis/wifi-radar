namespace API.Services.Interfaces.Auth;

public interface IPasswordHelper
{
    string HashPassword(Domain.Models.User user, string password);
    bool VerifyPassword(Domain.Models.User user, string hashedPassword, string password);
}