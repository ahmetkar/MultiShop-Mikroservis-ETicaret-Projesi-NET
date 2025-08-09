using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.Interfaces;
using System.Security.Claims;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        public OrderAddressService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,IUserService userService)
        {
            _httpClient = httpClient;
            _userService = userService;
        }
        public async Task<int> CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateOrderAddressDto>("adresses", createOrderAddressDto);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CreateAdressResultDto>();
                return json.AdressId;
            }

            return 0;
        }

        public async Task<List<ResultOrderAddressDto>> GetUserAddressesByUserIdAsync()
        {
            var userId = await _userService.GetUserId();

            var response = await _httpClient.GetAsync($"adresses/GetAdressesByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
               var adresses =  await response.Content.ReadFromJsonAsync<List<ResultOrderAddressDto>>();
                return adresses; 
            }

            return new List<ResultOrderAddressDto>();
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