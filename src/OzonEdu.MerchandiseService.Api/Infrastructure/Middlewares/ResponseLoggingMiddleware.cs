using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Api.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            LogResponse(context);
        }

        private void LogResponse(HttpContext context)
        {
            try
            {
                if (context.Request.ContentType == "application/grpc")
                    return;
                
                var headers = JsonSerializer.Serialize(context.Response.Headers);
                _logger.LogInformation($"Logging http response from middleware\nResponse headers: {headers}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log response");
            }
        }
    }
}