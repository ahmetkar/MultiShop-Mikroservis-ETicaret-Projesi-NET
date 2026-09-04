using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public interface IOrderOderingService
    {
        Task<List<ResultOrderingByUserIdDto>> GetAllOrderingsAsync();
        Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id);
        Task<bool> SetOrderingStatus(int orderingId, bool newStatus);
        Task<bool> UpdateOrderStatusAsync(int orderingId, OrderStatus status);
        Task<ResultOrderingByUserIdDto?> GetActiveOrderingByUserId(string id);
        Task<GetOrderingByIdResultDto> GetOrderingById(int id);
        Task<CreateOrderingResultDto?> CreateOrdering(int billingAdressId, int shippingAdressId);
        Task<bool> DeleteOrdering(int orderingId);
    }
}
