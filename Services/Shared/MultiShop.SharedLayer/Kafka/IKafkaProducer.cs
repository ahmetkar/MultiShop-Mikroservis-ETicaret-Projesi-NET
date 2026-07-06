namespace MultiShop.SharedLayer.Kafka
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(string topic,T message,string key,CancellationToken cancellationToken=default);
    }
}
