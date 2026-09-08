namespace MultiShop.SharedLayer.Events
{
    public class OrderCancelledEvent : IntegrationEvent
    {
        public int OrderingId { get; set; }
        public string? UserId { get; set; }
        public string? Reason { get; set; }
    }
}
