using System.Text.RegularExpressions;

namespace API.Helpers;

public static class UserValidator
{
    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$";
        
        return Regex.IsMatch(password, pattern);
    }
}