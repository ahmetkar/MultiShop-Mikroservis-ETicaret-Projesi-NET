namespace MultiShop.Payment.Consumers
{

    using Confluent.Kafka;
    using MultiShop.SharedLayer.Kafka;
    using MultiShop.SharedLayer.Events;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Text.Json;
   
    using Microsoft.EntityFrameworkCore;
    using MultiShop.Payment.Services;
    using MultiShop.Payment.DAL.Context;
    using MultiShop.Payment.DAL.Entities;

    public class OrderCreatedConsumer : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<OrderCreatedConsumer> logger)
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
                    GroupId = "payment-service-group-order-created",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,

                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.OrderCreated);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var message = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);

                        if (message is null)
                        {
                            consumer.Commit(result);
                            continue;
                        }

                        using var scope = _serviceProvider.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();

                        var exists = await dbContext.PaymentOrderSnapshots.AnyAsync(x=>x.OrderingId
                         ==message.OrderingId,stoppingToken);

                        if (!exists)
                        {
                            var snapshot = new PaymentOrderSnapshot
                            {
                                OrderingId = message.OrderingId,
                                UserId = message.UserId,
                                PaymentTotal = message.PaymentTotal,
                                CorrelationId = message.CorrrelationId,
                                IsSuccessful = false,
                                CreatedDate = DateTime.UtcNow
                            };

                            await dbContext.PaymentOrderSnapshots.AddAsync(snapshot,stoppingToken);
                            await dbContext.SaveChangesAsync(stoppingToken);

                            consumer.Commit(result);
                        }

                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "OrderCreated consumer hata aldı.");
                    }

                }

                consumer.Close();
            }, stoppingToken
            );



        }
    }
}
