using Microsoft.AspNetCore.Http;
namespace CodeLatheeshAPI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var errorResponse = new { message = "An unexpected error occurred." };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
