using Serilog;
using System.Text.Json;

namespace AdformAPI.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = JsonSerializer.Serialize(new
                {
                    status = ex.StatusCode, // Left the status code, so that it can be used with the message for the user
                    error = ex.Message
                });

                await context.Response.WriteAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = JsonSerializer.Serialize(new
                {
                    status = 500,
                    error = "An unexpected error occurred"
                });

                await context.Response.WriteAsync(response);
            }
        }
    }
}
