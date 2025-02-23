using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Common;

namespace TaskManagementAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro não tratado.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.";

            if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "Recurso não encontrado.";
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status403Forbidden;
                message = "Acesso não autorizado.";
            }

            var errorDetails = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
                StackTrace = exception.StackTrace
            };

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorDetails));
        }
    }
}
