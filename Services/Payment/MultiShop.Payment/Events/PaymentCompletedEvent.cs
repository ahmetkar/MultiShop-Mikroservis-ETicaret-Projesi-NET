namespace MultiShop.Payment.Events
{
    public class PaymentCompletedEvent: IntegrationEvent
    {
        public int OrderingId { get; set; }
        public int PaymentTotal { get; set; }
        public string UserId { get; set; }
        public string PaymentType { get; set; }
    }
}
