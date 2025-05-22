using API.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Auth;

public class PasswordService : IPasswordHelper
{
    private readonly IPasswordHasher<Domain.Models.User> _passwordHasher;
    
    public PasswordService(IPasswordHasher<Domain.Models.User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public string HashPassword(Domain.Models.User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(Domain.Models.User user, string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        
        return result == PasswordVerificationResult.Success;
    }
}