namespace MultiShop.Payment.Events
{
    public class PaymentFailedEvent : IntegrationEvent
    {
        public int OrderingId { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
