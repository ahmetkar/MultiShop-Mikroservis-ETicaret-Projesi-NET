namespace MultiShop.SharedLayer.Events
{
    public class OrderNotCreatedEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
