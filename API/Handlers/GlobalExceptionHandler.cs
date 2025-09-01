using API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Handlers
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = exception switch
            {
                InvalidOperationException => ProblemDetailsGenerator.BadRequest(exception.Message),
                NotFoundException => ProblemDetailsGenerator.NotFound(exception.Message),
                ConflictException => ProblemDetailsGenerator.Conflict(exception.Message),
                _ => ProblemDetailsGenerator.InternalServerError()
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        public static class ProblemDetailsGenerator
        {
            public static ProblemDetails BadRequest(string message) => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = message
            };

            public static ProblemDetails NotFound(string message) => new()
            {
                Status = StatusCodes.Status404NotFound,
                Title = message
            };

            public static ProblemDetails Conflict(string message) => new()
            {
                Status = StatusCodes.Status409Conflict,
                Title = message
            };

            public static ProblemDetails InternalServerError() => new()
            {
                Status = StatusCodes.Status500InternalServerError
            };
        }
    }
}
