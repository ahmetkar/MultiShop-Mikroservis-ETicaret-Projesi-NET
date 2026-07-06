namespace MultiShop.SharedLayer.Events
{
    public class CargoDetailCreated : IntegrationEvent
    {

        public int CargoDetailId { get; set; }
        public int CustomerId { get; set; }

        public int CargoCompanyId { get; set; }

        public string ReceiverCustomer { get; set; }


    }
}
