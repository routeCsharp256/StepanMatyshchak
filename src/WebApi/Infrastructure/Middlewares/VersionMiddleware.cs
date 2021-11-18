using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OzonEdu.MerchandiseService.Api.Infrastructure.Models;

namespace OzonEdu.MerchandiseService.Api.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
            var versionInfo = new VersionInfo { Version = version, ServiceName = serviceName};
            var versionInfoJson = JsonSerializer.Serialize(versionInfo);
            await context.Response.WriteAsync(versionInfoJson);
        }
    }
}