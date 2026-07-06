namespace MultiShop.SharedLayer.Events
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid CorrrelationId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
