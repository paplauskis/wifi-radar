namespace API.Domain.Dto;

public class UserLoginResponseDto
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}