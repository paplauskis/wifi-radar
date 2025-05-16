using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace API.Domain.Models;

public class User : TimeStampedEntity
{
    public User() { }

    [SetsRequiredMembers]
    public User(string username, string password)
    {
        Username = username;
        Password = password;

        Validate();
    }

    [BsonElement("Username")]
    public required string Username { get; set; }

    [BsonElement("Password")]
    public required string Password { get; set; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(Password))
            throw new ArgumentException("Password is required.");

        if (Password.Length > 20)
            throw new ArgumentException("Password must be 20 characters or less.");

        if (!Password.Any(char.IsUpper))
            throw new ArgumentException("Password must include at least one uppercase letter.");

        if (!Password.Any(char.IsDigit))
            throw new ArgumentException("Password must include at least one digit.");

        if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new ArgumentException("Password must include at least one special character.");
    }

    private string? _fullUsername;
    public string FullUsername => _fullUsername ??= $"{Username} [Registered]";
}