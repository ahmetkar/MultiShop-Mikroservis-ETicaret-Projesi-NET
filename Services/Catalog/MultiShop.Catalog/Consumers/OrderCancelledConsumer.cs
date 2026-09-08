using Confluent.Kafka;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.OrderDetailDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Services.ProductServices;
using MultiShop.Catalog.Settings;
using MultiShop.SharedLayer.Events;
using MultiShop.SharedLayer.Kafka;
using System.Net.Http.Json;
using System.Text.Json;

namespace MultiShop.Catalog.Consumers
{
    public class OrderCancelledConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderCancelledConsumer> _logger;

        public OrderCancelledConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<OrderCancelledConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:19095",
                    GroupId = "catalog-order-cancelled-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                    AllowAutoCreateTopics = true
                };

                using var consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(KafkaTopics.OrderCancelled);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        if (result?.Message?.Value == null) continue;

                        var message = JsonSerializer.Deserialize<OrderCancelledEvent>(result.Message.Value);
                        if (message == null) { consumer.Commit(result); continue; }

                        using var scope = _serviceProvider.CreateScope();
                        var dbSettings = scope.ServiceProvider.GetRequiredService<IDatabaseSettings>();
                        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                        var httpFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

                        var client = new MongoClient(dbSettings.ConnectionStrings);
                        var db = client.GetDatabase(dbSettings.DatabaseName);
                        var col = db.GetCollection<ProcessedEvent>(string.IsNullOrEmpty(dbSettings.ProcessedEventCollectionName) ? "ProcessedEvents" : dbSettings.ProcessedEventCollectionName);

                        var eventIdStr = message.EventId.ToString();
                        var processed = await col.Find(x => x.EventId == eventIdStr).AnyAsync(stoppingToken);
                        if (processed) { consumer.Commit(result); continue; }

                        await RestoreStockAsync(message.OrderingId, productService, httpFactory, stoppingToken);

                        await col.InsertOneAsync(new ProcessedEvent
                        {
                            EventId = eventIdStr,
                            HandlerName = nameof(OrderCancelledConsumer),
                            ProcessedAt = DateTime.UtcNow
                        }, cancellationToken: stoppingToken);

                        consumer.Commit(result);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogWarning("Kafka topic error: {Reason}", ex.Error.Reason);
                        await Task.Delay(5000, stoppingToken);
                    }
                    catch (OperationCanceledException) { break; }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "OrderCancelledConsumer error");
                        await Task.Delay(3000, stoppingToken);
                    }
                }
                consumer.Close();
            }, stoppingToken);
        }

        private async Task RestoreStockAsync(int orderingId, IProductService productService, IHttpClientFactory httpFactory, CancellationToken ct)
        {
            var orderServiceUrl = _configuration["OrderServiceUrl"] ?? "http://localhost:7072";
            var httpClient = httpFactory.CreateClient();
            var url = $"{orderServiceUrl}/api/OrderDetail/{orderingId}";

            try
            {
                var response = await httpClient.GetAsync(url, ct);
                if (response.IsSuccessStatusCode)
                {
                    var items = await response.Content.ReadFromJsonAsync<List<OrderDetailConsumerDto>>(cancellationToken: ct);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            var filterList = new List<string>();
                            if (!string.IsNullOrEmpty(item.ProductFilters))
                            {
                                var splits = item.ProductFilters.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var s in splits) filterList.Add(s.Trim());
                            }
                            int amount = item.ProductAmount > 0 ? item.ProductAmount : 1;
                            await productService.IncreaseProductFilterStockAsync(item.ProductId, filterList.Distinct().ToList(), amount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring stock for cancelled order {OrderingId}", orderingId);
            }
        }
    }
}