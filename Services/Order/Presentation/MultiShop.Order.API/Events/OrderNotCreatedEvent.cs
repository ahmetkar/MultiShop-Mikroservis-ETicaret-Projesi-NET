namespace MultiShop.Order.API.Events
{
    public class OrderNotCreatedEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string OrderingId { get; set; }

        public decimal TotalPrice { get; set; }

        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
