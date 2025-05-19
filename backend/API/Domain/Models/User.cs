using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;

namespace API.Domain.Models
{
    public class User : TimeStampedEntity
    {
        private string _username;

        [BsonElement("Username")]
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username is required.");
                if (value.Length > 20)
                    throw new ArgumentException("Username is too long, maximum length is 20");
                _username = value;
            }
        }

        [BsonElement("Password")]
        public string Password { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username is required.");

            if (Username.Length > 20)
                throw new ArgumentException("Username must be 20 characters or fewer.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Password is required.");

            if (Password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (Password.Length > 20)
                throw new ArgumentException("Password must be 20 characters or fewer.");

            if (!Password.Any(char.IsUpper))
                throw new ArgumentException("Password must include at least one uppercase letter.");

            if (!Password.Any(char.IsLower))
                throw new ArgumentException("Password must include at least one lowercase letter.");

            if (!Password.Any(char.IsDigit))
                throw new ArgumentException("Password must include at least one digit.");

            if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new ArgumentException("Password must include at least one special character.");
        }
    }
}