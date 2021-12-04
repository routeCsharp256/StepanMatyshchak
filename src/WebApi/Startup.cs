using Application.Repositories;
using Infrastructure.Configuration;
using Infrastructure.MessageBroker;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Repositories.Infrastructure;
using Infrastructure.Repositories.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTracing;
using OzonEdu.MerchandiseService.Api.GrpcServices;
using OzonEdu.MerchandiseService.Api.HostedServices;
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
            AddHostedServices(services);
            AddMediator(services);
            AddDatabaseComponents(services);
            AddRepositories(services);
            AddKafkaServices(services, Configuration);
            
            // Change to mediator
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
        
        private static void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(DatabaseConnectionOptions));
        }

        private void AddDatabaseComponents(IServiceCollection services)
        {
            services.Configure<DatabaseConnectionOptions>(Configuration.GetSection(nameof(DatabaseConnectionOptions)));
            
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<IMerchandiseRequestRepository, MerchandiseRequestRepository>();
            services.AddScoped<IMerchPackRepository, MerchPackRepository>();
        }
        
        private static void AddKafkaServices(IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddSingleton<IProducerBuilderWrapper, ProducerBuilderWrapper>();
        }
        
        private static void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<EmployeeConsumerHostedService>();
            services.AddHostedService<StockConsumerHostedService>();
        }
        
        private static void AddOpenTracing(IServiceCollection services)
        {
            // services.AddSingleton<ITracer>(
            //     sd =>
            //     {
            //         var tracer = new Tracer.Builder("MerchandiseService");
            //     });
        }
    }
}