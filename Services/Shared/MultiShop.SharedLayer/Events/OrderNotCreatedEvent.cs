namespace MultiShop.SharedLayer.Events
{
    public class OrderNotCreatedEvent : IntegrationEvent
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
