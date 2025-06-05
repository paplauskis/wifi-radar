using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data.Repositories.Interfaces;
using API.Domain.Dto;
using API.Services.Interfaces.Auth;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Services.Auth;

public class JwtService : IAuthenticatable
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHelper _passwordHelper;

    public JwtService(IUserRepository repository, IConfiguration configuration, IPasswordHelper passwordHelper)
    {
        _repository = repository;
        _configuration = configuration;
        _passwordHelper = passwordHelper;
    }
    
    public async Task<UserLoginResponseDto?> AuthenticateUser(UserLoginRequestDto userRequestDto)
    {
        var user = await GetAuthenticatedUser(userRequestDto);
        if (user == null) return null;
        
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = _configuration["JwtConfig:Key"];
        var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpiryDate = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = tokenExpiryDate,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        return new UserLoginResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            AccessToken = accessToken,
            ExpiresIn = (int)tokenExpiryDate.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }

    private async Task<Domain.Models.User?> GetAuthenticatedUser(UserLoginRequestDto userRequestDto)
    {
        if (string.IsNullOrEmpty(userRequestDto.Username) || string.IsNullOrEmpty(userRequestDto.Password))
        {
            return null;
        }
        
        var user = await _repository.GetByUsernameAsync(userRequestDto.Username);
        if (user == null) return null;
        
        var isPasswordValid = _passwordHelper.VerifyPassword(user, user.Password, userRequestDto.Password);
        if (isPasswordValid == false)
            return null;

        return user;
    }
}