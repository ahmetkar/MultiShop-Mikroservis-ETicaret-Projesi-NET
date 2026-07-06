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

    public class PaymentFailedConsumer : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<PaymentFailedConsumer> logger)
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
                    GroupId = "order-service-group-payment-failed",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,

                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.PaymentFailed);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var message = JsonSerializer.Deserialize<PaymentFailedEvent>(result.Message.Value);

                        if (message is null)
                        {
                            consumer.Commit(result);
                            continue;
                        }

                        using var scope = _serviceProvider.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();


                        var alreadyProcessed = await dbContext.ProcessedEvents
                          .AnyAsync(x => x.EventId == message.EventId, stoppingToken);

                        if (alreadyProcessed)
                        {
                            consumer.Commit(result);
                            continue;
                        }

                        var order = await dbContext.Orderings.FirstOrDefaultAsync(x => x.OrderingId == message.OrderingId, stoppingToken);

                        if (order is not null)
                        {
                            order.Status = OrderStatus.PaymentFailed;


                            await dbContext.ProcessedEvents.AddAsync(new ProcessedEvent
                            {
                                EventId = message.EventId,
                                HandlerName = nameof(PaymentFailedConsumer),
                                ProcessedAt = DateTime.UtcNow
                            }, stoppingToken);

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
                        _logger.LogError(ex, "PaymentFailed consumer hata aldı.");
                    }

                }

                consumer.Close();
            }, stoppingToken
            );



        }
    }
}
