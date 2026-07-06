namespace MultiShop.SharedLayer.Events
{
    public class CargoCustomerCreated : IntegrationEvent
    {

        
        public string UserCustomerId { get; set; }

        public int CargoCustomerId { get; set; }
    }
}
