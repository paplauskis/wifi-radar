using System.Net;
using System.Text.Json;
using API.Domain.Exceptions;
using API.Models;

namespace API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        
        var (errorResponse, statusCode) = exception switch
        {
            // Bad Request (400)
            InvalidInputException => 
                (ApiResponse<object>.Error(exception.Message), HttpStatusCode.BadRequest),
            
            // Unauthorized (401)
            InvalidCredentialsException => 
                (ApiResponse<object>.Error(exception.Message), HttpStatusCode.Unauthorized),
            
            // Not Found (404)
            NotFoundException => 
                (ApiResponse<object>.Error(exception.Message), HttpStatusCode.NotFound),
            
            // Conflict (409)
            ConflictException => 
                (ApiResponse<object>.Error(exception.Message), HttpStatusCode.Conflict),
            
            // Internal Server Error (500)
            _ => (ApiResponse<object>.Error("An unexpected error occurred"), HttpStatusCode.InternalServerError)
        };

        response.StatusCode = (int)statusCode;
        
        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        
        await response.WriteAsync(json);
        
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "An unexpected error occurred");
        }
    }
} 