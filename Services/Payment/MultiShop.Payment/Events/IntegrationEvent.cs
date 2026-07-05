namespace MultiShop.Payment.Events
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; set; }
        public Guid CorrrelationId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
