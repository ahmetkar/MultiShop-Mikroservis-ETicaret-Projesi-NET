using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public interface IOrderOderingService
    {
        Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id);
        Task<bool> SetOrderingStatus(int orderingId, bool newStatus);
        Task<ResultOrderingByUserIdDto?> GetActiveOrderingByUserId(string id);
        Task<GetOrderingByIdResultDto> GetOrderingById(int id);
        Task<int?> CreateOrdering(int billingAdressId, int shippingAdressId);
        Task<bool> DeleteOrdering(int orderingId);
    }
}