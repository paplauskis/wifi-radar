namespace API.Domain.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public string? Username { get; set; }
    
    public UserAlreadyExistsException(string message) : base(message) { }
}