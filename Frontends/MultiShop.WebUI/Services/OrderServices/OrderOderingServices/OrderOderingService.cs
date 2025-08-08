using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public class OrderOderingService : IOrderOderingService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
       
        private readonly IBasketService _basketService;

        public OrderOderingService(HttpClient httpClient,
            IUserService userService,IBasketService basketService)
        {
            _httpClient = httpClient;
            _userService = userService;
            _basketService = basketService;
        }
        public async Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetOrderingByUserId/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsonData);
            return values;
        }

        public async Task CreateOrdering(int orderAdressId)
        {
            var values = await _userService.GetUserInfo();

            var basket = await _basketService.GetBasket();
            //CREATE ORDERING 
            CreateOrderingDto createOrderingDto = new CreateOrderingDto()
            {
                BillingAddressId = 0,
                ShippingAdressId = 0,
                OrderDate = DateTime.Now,
                TotalPrice = basket.TotalPrice,
                UserId = values.Id
            };
            var response = await _httpClient.PostAsJsonAsync<CreateOrderingDto>("Orderings", createOrderingDto);
            if (response.IsSuccessStatusCode)
            {
                //EĞER CreateOrdering BAŞARILI İSE
                //CREATE ORDER DETAİL
   
                



            }
          


        }


    }
}