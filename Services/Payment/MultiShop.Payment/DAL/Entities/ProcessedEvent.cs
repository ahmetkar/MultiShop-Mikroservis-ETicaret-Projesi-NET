namespace MultiShop.Payment.DAL.Entities
{
    public class ProcessedEvent
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }

        public string HandlerName { get; set; } = string.Empty;

        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}
