using Serilog;
using System.Text.Json;

namespace AdformAPI.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ApiException ex)
            {
                Log.Error(ex.Message);

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
                Log.Error(ex, ex.Message);
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
