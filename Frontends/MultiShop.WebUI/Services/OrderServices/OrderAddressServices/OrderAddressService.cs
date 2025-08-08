using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        private readonly HttpClient _httpClient;
        public OrderAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> CreateOrderAddressAsync(CreateOrderAddressDto createOrderAddressDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateOrderAddressDto>("addresses", createOrderAddressDto);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<CreateAdressResultDto>();
                return json.AdressId;
            }

            return 0;
        }
    }
}