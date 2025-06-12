using System.Text.Json;

namespace API.Models;

/// Standard API response envelope for consistent error handling and HATEOAS support
/// <typeparam name="T">Type of the data payload</typeparam>
public class ApiResponse<T>
{
    /// Indicates whether the request was successful
    public bool Success { get; set; }

    /// The actual data payload
    public T? Data { get; set; }

    /// Human-readable message about the operation result
    public string? Message { get; set; }
    
    /// List of error messages if the operation failed
    public List<string>? Errors { get; set; }

    /// HATEOAS links for resource navigation
    public Dictionary<string, string>? Links { get; set; }

    /// Creates a successful response with optional message and HATEOAS links
    public static ApiResponse<T> Ok(T data, string? message = null, Dictionary<string, string>? links = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Links = links
        };
    }

    // Creates an error response with a message and optional detailed errors
    public static ApiResponse<T> Error(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
} 