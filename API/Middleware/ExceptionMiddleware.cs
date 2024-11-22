using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred for request {Path}", context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Set response status code based on exception type
            var statusCode = exception switch
            {
                
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError // Default to 500
            };

            context.Response.StatusCode = statusCode;

            // Build response object
            var response = new ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred",
                Detail = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred.",
                Instance = context.Request.Path // Include the request path
            };

            if (_env.IsDevelopment() && exception.StackTrace != null)
            {
                // Add stack trace in development mode for debugging
                response.Extensions["stackTrace"] = exception.StackTrace;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Serialize and write the response
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
