using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Server.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KnowledgeShare.Server.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                var errorsString = JsonSerializer.Serialize(exception.Errors);

                _logger.LogError(errorsString);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(errorsString);

                return;
            }
            catch (AuthorizationException exception)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                var errorString = JsonSerializer.Serialize(exception.Failure);

                _logger.LogError(errorString);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                return;
            }
        }
    }
}
