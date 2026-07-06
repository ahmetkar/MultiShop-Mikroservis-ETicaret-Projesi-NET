namespace MultiShop.SharedLayer.Events
{
    public class PaymentCompletedEvent: IntegrationEvent
    {
        public int OrderingId { get; set; }
        public int PaymentTotal { get; set; }
        public string UserId { get; set; }
        public int PaymentId { get; set; }

        public int CargoCompanyId { get; set; }

        public int CardInfoId { get; set; }
    }
}
