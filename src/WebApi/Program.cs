using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchandiseService.Api;
using OzonEdu.MerchandiseService.Api.Infrastructure.Extensions;
using Serilog;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) => configuration
            .ReadFrom
            .Configuration(context.Configuration)
            .WriteTo.Console())
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
        .AddInfrastructure()
        .AddHttp();
