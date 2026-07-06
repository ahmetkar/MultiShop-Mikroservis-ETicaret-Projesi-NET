namespace MultiShop.SharedLayer.Events
{
    public class CargoOperationDelivered : IntegrationEvent
    {
        public int CargoOperationId { get; set; }

        public int OrderingId { get; set; }
        public DateTime OperationDate { get; set; }

        public int CargoDetailId { get; set; }

    }
}
