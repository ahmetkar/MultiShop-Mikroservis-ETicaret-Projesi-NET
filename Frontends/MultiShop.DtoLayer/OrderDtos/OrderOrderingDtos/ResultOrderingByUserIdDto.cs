using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos
{
    public class ResultOrderingByUserIdDto
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsOrderCompleted { get; set; } = false;
        public bool IsOrderDelivered { get; set; } = false;
        public DateTime OrderDate { get; set; }
        public int ShippingAdressId { get; set; }
        public int BillingAddressId { get; set; }
    }
}