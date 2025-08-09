using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    { // Task<List<ResultAboutDto>> GetAllAboutAsync();
        Task<int> CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto);
        Task<List<ResultOrderAddressDto>> GetUserAddressesByUserIdAsync();
        Task<int> GetUserAdressCount();
        Task<int> GetUserBillingAdressCount();
        Task<int> GetUserShippingAdressCount();
        //    Task UpdateAboutAsync(UpdateAboutDto updateAboutDto);
        //    Task DeleteAboutAsync(string id);
        //    Task<UpdateAboutDto> GetByIdAboutAsync(string id);
    }
}
