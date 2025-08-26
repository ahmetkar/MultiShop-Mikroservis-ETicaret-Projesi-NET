using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public interface IOrderOderingService
    {
        Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id);

        Task<ResultOrderingByUserIdDto?> GetActiveOrderingByUserId(string id);
        Task CreateOrdering(int billingAdressId, int shippingAdressId);
    }
}