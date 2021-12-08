using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.NewMerchandiseAppeared;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using Infrastructure.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OzonEdu.MerchandiseService.Api.HostedServices
{
    public class StockConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockConsumerHostedService> _logger;

        protected string Topic { get; set; } = "stock_replenished_event";
        
        public StockConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<StockConsumerHostedService> logger)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
            };

            using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                c.Subscribe(Topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var sw = new Stopwatch();
                            try
                            {
                                await Task.Yield();
                                sw.Start();
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                var cr = c.Consume(stoppingToken);
                                if (cr != null)
                                {
                                    var message = JsonSerializer.Deserialize<StockReplenishedEvent>(cr.Message.Value);
                                    await mediator.Send(new NewMerchandiseAppearedCommand()
                                    {
                                        SkuCollection = message?.Type.Select(x => x.Sku).ToList()
                                    }, stoppingToken);
                                }
                            }
                            catch (Exception ex)
                            {
                                sw.Stop();
                                _logger.LogError($"Error while get consume. Message {ex.Message}");
                            }
                        }
                    }
                }
                finally
                {
                    c.Commit();
                    c.Close();
                }
            }
        }
    }
}