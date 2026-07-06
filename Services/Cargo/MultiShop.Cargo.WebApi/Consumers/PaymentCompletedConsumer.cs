namespace MultiShop.Cargo.WebApi.Consumers
{

    using Confluent.Kafka;
    using MultiShop.SharedLayer.Kafka;
    using MultiShop.SharedLayer.Events;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Text.Json;

    using Microsoft.EntityFrameworkCore;
    using MultiShop.Cargo.BussinessLayer.Abstract;
    using EntityLayer.Concretes;
    using MultiShop.Cargo.DataAccessLayer.Concrete;

    public class PaymentCompletedConsumer : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentCompletedConsumer> _logger;

        public PaymentCompletedConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<PaymentCompletedConsumer> logger)
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
                    GroupId = "cargo-service-group-payment-completed",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,

                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.PaymentCompleted);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var message = JsonSerializer.Deserialize<PaymentCompletedEvent>(result.Message.Value);

                        if (message is null)
                        {
                            consumer.Commit(result);
                            continue;
                        }

                        using var scope = _serviceProvider.CreateScope();

                        var kafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();
                        var dbContext =scope.ServiceProvider.GetRequiredService<CargoContext>();

                        var alreadyProcessed = await dbContext.ProcessedEvents
                               .AnyAsync(x => x.EventId == message.EventId, stoppingToken);

                        if (alreadyProcessed)
                        {
                            consumer.Commit(result);
                            continue;
                        }


                        try
                        {

                            var cargoOpService = scope.ServiceProvider.GetRequiredService<ICargoOperationService>();
                            var cargoCusService = scope.ServiceProvider.GetRequiredService<ICargoCustomerService>();
                            var cargoDetailService = scope.ServiceProvider.GetRequiredService<ICargoDetailService>();

                            

                            var resCus = cargoCusService.TInsert(new CargoCustomer() { UserCustomerId = message.UserId });
                            if (resCus is not null)
                            {
                                var resDetail = cargoDetailService.TInsert(new CargoDetail()
                                {
                                    CustomerId = resCus.CargoCustomerId,
                                    CargoCompanyId = message.CargoCompanyId,
                                    Barcode = "xxxx-xxxx-xxxx"
                                });

                                if (resDetail is not null)
                                {
                                    var resOp = cargoOpService.TInsert(new CargoOperation()
                                    {
                                        CargoDetailId = resDetail.CargoDetailId,
                                        OperationDate = DateTime.Now,
                                        OrderingId = message.OrderingId,
                                        Description = "Kargo oluşturuldu"
                                    });


                                    if (resOp is not null)
                                    {
                                        var cargoCreatedEvent = new CargoOperationCreated
                                        {
                                            CargoDetailId = resOp.CargoDetailId,
                                            CargoOperationId = resOp.CargoOperationId,
                                            CreatedDate = resOp.OperationDate,
                                            OrderingId = resOp.OrderingId,

                                        };
                                        await kafkaProducer.PublishAsync(KafkaTopics.CargoCreated, cargoCreatedEvent, resOp.CargoOperationId.ToString(), stoppingToken);

                                        await dbContext.ProcessedEvents.AddAsync(new ProcessedEvent
                                        {
                                            EventId = message.EventId,
                                            HandlerName = nameof(PaymentCompletedConsumer),
                                            ProcessedAt = DateTime.UtcNow
                                        }, stoppingToken);

                                        await dbContext.SaveChangesAsync(stoppingToken);

                                        consumer.Commit(result);

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            var cargoFailedEvent = new CargoOperationFailed
                            {
                                OrderingId = message.OrderingId,
                                UserId = message.UserId,
                                PaymentId = message.PaymentId
                            };

                            await kafkaProducer.PublishAsync(KafkaTopics.CargoFailed,cargoFailedEvent,message.OrderingId.ToString(),stoppingToken);
                            
                        }

                        

                        consumer.Commit(result);
                        

                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "PaymentCompleted consumer hata aldı.");
                    }

                }

                consumer.Close();
            }, stoppingToken
            );



        }
    }
}
