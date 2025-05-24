using MCComputersBackend.Exceptions;
using System.Net;
using System.Text.Json;

namespace MCComputersBackend.Middleware
{
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                EntityNotFoundException => new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = exception.Message
                },
                InsufficientStockException => new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = exception.Message
                },
                BusinessLogicException => new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = exception.Message
                },
                _ => new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An internal server error occurred"
                }
            };

            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
