using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Api.GrpcServices;
using OzonEdu.MerchandiseService.Api.Infrastructure.Interceptors;
using OzonEdu.MerchandiseService.Api.Services;
using OzonEdu.MerchandiseService.Api.Services.Interfaces;

namespace OzonEdu.MerchandiseService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMerchService, MerchService>();
            
            services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchandiseGrpcService>();
                endpoints.MapControllers();
            });
        }
    }
}