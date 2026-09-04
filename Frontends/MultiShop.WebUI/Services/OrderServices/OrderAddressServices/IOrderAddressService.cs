using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        Task<int> CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        Task<List<ResultOrderAddressDto>> GetUserAddressesByUserIdAsync();
        Task<ResultOrderAddressDto?> GetAddressByIdAsync(int addressId);
        Task UpdateOrderAddressAsync(UpdateOrderAddressDto updateOrderAddressDto);
        Task DeleteOrderAddressAsync(int addressId);
        Task<int> GetUserAdressCount();
        Task<int> GetUserBillingAdressCount();
        Task<int> GetUserShippingAdressCount();
    }
}
