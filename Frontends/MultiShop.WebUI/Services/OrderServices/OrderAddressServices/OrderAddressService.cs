using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        public OrderAddressService(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            _userService = userService;
        }

        public async Task<int> CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto)
        {
            if (string.IsNullOrWhiteSpace(createOrderAddressDto.UserId))
            {
                var userId = await _userService.GetUserId();
                createOrderAddressDto.UserId = userId;
            }

            var response = await _httpClient.PostAsJsonAsync("adresses", createOrderAddressDto);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CreateAdressResultDto>();
                return json?.AdressId ?? 0;
            }

            return 0;
        }

        public async Task<List<ResultOrderAddressDto>> GetUserAddressesByUserIdAsync()
        {
            var userId = await _userService.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<ResultOrderAddressDto>();
            }

            var response = await _httpClient.GetAsync($"adresses/GetAdressesByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var adresses = await response.Content.ReadFromJsonAsync<List<ResultOrderAddressDto>>();
                return adresses ?? new List<ResultOrderAddressDto>();
            }

            return new List<ResultOrderAddressDto>();
        }

        public async Task<ResultOrderAddressDto?> GetAddressByIdAsync(int addressId)
        {
            var addresses = await GetUserAddressesByUserIdAsync();
            return addresses.FirstOrDefault(x => x.AdressId == addressId);
        }

        public async Task UpdateOrderAddressAsync(UpdateOrderAddressDto updateOrderAddressDto)
        {
            if (string.IsNullOrWhiteSpace(updateOrderAddressDto.UserId))
            {
                var userId = await _userService.GetUserId();
                updateOrderAddressDto.UserId = userId;
            }

            await _httpClient.PutAsJsonAsync("adresses", updateOrderAddressDto);
        }

        public async Task DeleteOrderAddressAsync(int addressId)
        {
            await _httpClient.DeleteAsync($"adresses?id={addressId}");
        }

        public async Task<int> GetUserAdressCount()
        {
            var adresses = await this.GetUserAddressesByUserIdAsync();
            return adresses.Count;
        }

        public async Task<int> GetUserBillingAdressCount()
        {
            var adresses = await this.GetUserAddressesByUserIdAsync();
            var newAdresses = adresses.Where(x => x.IsBillingOrShipping == true).ToList();
            return newAdresses.Count;
        }

        public async Task<int> GetUserShippingAdressCount()
        {
            var adresses = await this.GetUserAddressesByUserIdAsync();
            var newAdresses = adresses.Where(x => x.IsBillingOrShipping == false).ToList();
            return newAdresses.Count;
        }
    }
}