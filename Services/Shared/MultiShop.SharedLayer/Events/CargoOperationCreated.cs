namespace MultiShop.SharedLayer.Events
{
    public class CargoOperationCreated : IntegrationEvent
    {
        public int CargoOperationId { get; set; }

        public int OrderingId { get; set; }
        public DateTime OperationDate { get; set; }

        public int CargoDetailId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
