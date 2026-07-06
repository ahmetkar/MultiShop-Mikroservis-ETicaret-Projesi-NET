namespace MultiShop.Order.API.Consumers
{

    using Confluent.Kafka;
    using MultiShop.SharedLayer.Kafka;
    using MultiShop.SharedLayer.Events;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Text.Json;
    using MultiShop.Order.Persistance.Context;
    using Microsoft.EntityFrameworkCore;
    using MultiShop.Order.Domain.Entities;

    public class CargoFailedConsumer : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CargoFailedConsumer> _logger;

        public CargoFailedConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<CargoFailedConsumer> logger)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _logger = logger;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _configuration["Kafka:BootstrapServers"],
                    GroupId = "order-service-group-cargo-failed",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,

                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.CargoFailed);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var message = JsonSerializer.Deserialize<CargoOperationFailed>(result.Message.Value);

                        if (message is null)
                        {
                            consumer.Commit(result);
                            continue;
                        }

                        using var scope = _serviceProvider.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();

                        var order = await dbContext.Orderings.FirstOrDefaultAsync(x => x.OrderingId == message.OrderingId, stoppingToken);

                        if (order is not null)
                        {
                            order.Status = OrderStatus.CargoFailed;
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }

                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "CargoFailed consumer hata aldı.");
                    }

                }

                consumer.Close();
            }, stoppingToken
            );



        }
    }
}
