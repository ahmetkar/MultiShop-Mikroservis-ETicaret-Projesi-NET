namespace MultiShop.SharedLayer.Events
{
    public class OrderCreatedEvent : IntegrationEvent
    {
        public int UserId { get; set; }
        public int OrderingId { get; set; }

        public int PaymentTotal { get; set; }

        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
