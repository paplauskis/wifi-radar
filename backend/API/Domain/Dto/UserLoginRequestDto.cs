namespace API.Domain;

public class UserLoginRequestDto
{
    public string? Password { get; set; }
    public string? Email { get; set; }
}