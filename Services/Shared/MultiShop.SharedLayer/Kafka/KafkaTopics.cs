namespace MultiShop.SharedLayer.Kafka
{
    public class KafkaTopics
    {
        public const string OrderCreated = "order-created";
        public const string OrderNotCreated = "order-not-created";

        public const string PaymentCompleted = "payment-completed";
        public const string PaymentFailed = "payment-failed";
        public const string PaymentRefunded = "payment-refunded";

        public const string CargoCreated = "cargo-created";
        public const string CargoFailed = "cargo-failed";

        public const string Completed = "completed";
    }
}
