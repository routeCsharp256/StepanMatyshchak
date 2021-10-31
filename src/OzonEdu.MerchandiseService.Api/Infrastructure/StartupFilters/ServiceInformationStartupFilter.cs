using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchandiseService.Api.Infrastructure.Middlewares;

namespace OzonEdu.MerchandiseService.Api.Infrastructure.StartupFilters
{
    public class ServiceInformationStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                app.Map("/ready", builder => builder.UseMiddleware<IsReadyMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<IsLiveMiddleware>());

                next(app);
            };
        }
    }
}