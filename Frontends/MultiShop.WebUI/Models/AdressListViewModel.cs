using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.WebUI.Models
{
    public class AdressListViewModel
    {
        public List<ResultOrderAddressDto> resultOrderAddressDto;

        public int BillingAdressId { get; set; }
        public int ShippingAdressId { get; set; }

        public int BillingAdressCount { get; set; }
        public int ShippingAdressCount { get; set; }
    }
}
