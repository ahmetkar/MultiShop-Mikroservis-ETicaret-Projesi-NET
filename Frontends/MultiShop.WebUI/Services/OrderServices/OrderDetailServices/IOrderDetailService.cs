using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingId(int orderingId);
        Task<List<ResultOrderDetailDto>> GetAllOrderDetailsAsync();
    }
}

