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
                    GroupId = "payment-service-group-cargo-failed",
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
                        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentContext>();
                        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                        var kafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();


                        var alreadyProcessed = await dbContext.ProcessedEvents
                                 .AnyAsync(x => x.EventId == message.EventId, stoppingToken);

                        if (alreadyProcessed)
                        {
                            consumer.Commit(result);
                            continue;
                        }


                        var refundedPayment = await paymentService.RefundPayment(new DTOs.RefundPaymentDto
                        {
                            OrderingId = message.OrderingId,
                        },stoppingToken);

                        if (!refundedPayment.Item1)

                        {
                            var paymentRefundendEvent = new PaymentRefundedEvent
                            {
                                OrderingId = message.OrderingId,
                                PaymentId = message.PaymentId,
                                UserId = message.UserId,
                                CargoOperationId = message.CargoOperationId.Value
                            };

                            await kafkaProducer.PublishAsync(KafkaTopics.PaymentRefunded,paymentRefundendEvent,message.OrderingId.ToString(),stoppingToken);


                            await dbContext.ProcessedEvents.AddAsync(new ProcessedEvent
                            {
                                EventId = message.EventId,
                                HandlerName = nameof(CargoFailedConsumer),
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
                        _logger.LogError(ex, "CargoFailed consumer hata aldı.");
                    }

                }

                consumer.Close();
            }, stoppingToken
            );



        }
    }
}
