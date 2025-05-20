namespace API.Domain;

public class UserLoginResponseDto
{
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}