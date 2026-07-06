
using Confluent.Kafka;
using System.Text.Json;


namespace MultiShop.SharedLayer.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {

        private readonly ProducerConfig _producerConfig;
        public KafkaProducer(IConfiguration configuration) {

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                EnableIdempotence = true, // producer tarafında duplicate riski olmasın
                Acks = Acks.All // Mesajın broker tarafından onaylanmasını bekle
            };
        
        }
        public async Task PublishAsync<T>(string topic, T message,string key, CancellationToken cancellationToken = default)
        {
            using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
            var json = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<string, string> { 
                Key = key,
                Value = json
            };

            await producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
        }

            
    }
}
