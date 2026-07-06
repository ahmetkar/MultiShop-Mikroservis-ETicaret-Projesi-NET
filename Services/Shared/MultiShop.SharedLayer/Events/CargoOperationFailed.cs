namespace MultiShop.SharedLayer.Events
{
    public class CargoOperationFailed : IntegrationEvent
    {

        public int? CargoOperationId { get; set; }

        public int OrderingId { get; set; }

        public int PaymentId { get; set; }

        public string UserId { get; set; }

        public DateTime OperationDate { get; set; }

    }
}
