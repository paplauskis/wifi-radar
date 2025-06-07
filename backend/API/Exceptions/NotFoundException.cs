namespace API.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string username)
            : base($"User '{username}' not found.") { }
    }

    public class WifiNetworkNotFoundException : NotFoundException
    {
        public WifiNetworkNotFoundException(string wifiId)
            : base($"Wi-Fi network with ID '{wifiId}' not found.") { }
    }
}
