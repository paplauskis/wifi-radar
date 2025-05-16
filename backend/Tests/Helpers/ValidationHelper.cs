using System;
using System.Text.RegularExpressions;

namespace Tests.Helpers;

public static class ValidationHelper
{
	public static bool IsStringValid(string input, int minLength = 1, int maxLength = int.MaxValue)
	{
		return !string.IsNullOrWhiteSpace(input)
		&& input.Length >= minLength
		&& input.Length <= maxLength;
	}

	public static bool IsIntInRange(int value, int min = int.MinValue, int max = int.MaxValue)
	{
		return value >= min && value <= max;
	}

	public static bool IsLongValid(long value)
	{
		return value > 0;
	}

	public static bool IsValidPassword(string password)
	{
		if (password == null) return true;
		if (password.Length > 20) return false;

		bool hasUpper = password.Any(char.IsUpper);
		bool hasDigit = password.Any(char.IsDigit);
		bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

		return hasUpper && hasDigit && hasSpecial;
	}

	public static string ValidateWifiSpot(WifiNetwork wifiNetwork)
	{

		if (!IsStringValid(wifiNetwork.City)) return "City is required and must be a valid string.";
		if (!IsStringValid(wifiNetwork.Country)) return "Country is required and must be a valid string.";
		if (!IsStringValid(wifiNetwork.PlaceName)) return "Spot name is required and must be a valid string.";
		if (!IsStringValid(wifiNetwork.Street)) return "Street name is required and must be a valid string.";
		if (!IsIntInRange(wifiNetwork.PostalCode, 1, 99999)) return "ZIP code must be between 1 and 5 digits.";
		if (!IsIntInRange(wifiNetwork.BuildingNumber, 1)) return "Building number must be a positive integer.";
		if (!IsValidPassword(wifiNetwork.Password)) return "Password must be 20 characters or less.";
		if (!IsLongValid(wifiNetwork.UserID)) return "UserID must be a positive number.";
		if (!wifiNetwork.IsFree && wifiNetwork.Password == null) return "Password must be provided for non-free WiFi.";

		return null;
	}

	public static string ValidateReview(WifiReview wifiReview)
	{
		if (!IsStringValid(wifiReview.Comment, 1, 30)) return "Comment must be 1 to 30 characters long";
		if (!IsIntInRange(wifiReview.Rating, 1, 10)) return "Rating must be between 1 and 10";
		if (!IsLongValid(wifiReview.UserID)) return "UserID must be a positive number";
		if (!IsStringValid(wifiReview.WifiId)) return "WifiId is a required field";
		if (wifiReview.CreatedAt == default) return "Date is a required field";

		return null;
	}

	public static string ValidateUser(User user)
	{
		if (!IsStringValid(user.Username)) return "Username is a required field.";
		if (!IsStringValid(user.Password)) return "Password is a required field.";
		if (user.DateCreated == default) return "Date is a required field.";

		return null;
	}
}