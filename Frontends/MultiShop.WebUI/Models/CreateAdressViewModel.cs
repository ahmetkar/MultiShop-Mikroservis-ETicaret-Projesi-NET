using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Models
{
    public class CreateAdressViewModel
    {
        public CreateOrderAddressDto Shipping { get; set; }
        public CreateOrderAddressDto Billing { get; set; }
        public bool IsShippingExists { get; set; }
    }
}
