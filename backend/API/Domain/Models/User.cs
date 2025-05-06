namespace API.Domain.Models;

public class User : TimeStampedEntity
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}