namespace MultiShop.SharedLayer.Events
{
    public class PaymentRefundedEvent : IntegrationEvent
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public int PaymentId { get; set; }

        public int CargoOperationId { get; set; }

    }
}
