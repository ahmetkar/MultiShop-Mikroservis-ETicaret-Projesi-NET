using Microsoft.AspNetCore.Http;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public class OrderOderingService : IOrderOderingService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _basketService;
        private readonly ICargoCompanyService _cargoCompanyService;

        public OrderOderingService(
            HttpClient httpClient,
            IUserService userService,
            IBasketService basketService,
            ICargoCompanyService cargoCompanyService,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _userService = userService;
            _basketService = basketService;
            _cargoCompanyService = cargoCompanyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ResultOrderingByUserIdDto>> GetAllOrderingsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("orderings/OrderingList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsonData);
                return values ?? new List<ResultOrderingByUserIdDto>();
            }
            return new List<ResultOrderingByUserIdDto>();
        }

        public async Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetOrderingsByUserId/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsonData);
            return values ?? new List<ResultOrderingByUserIdDto>();
        }

        public async Task<GetOrderingByIdResultDto> GetOrderingById(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetOrderingById/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetOrderingByIdResultDto>(jsonData);
            return values;
        }

        public async Task<ResultOrderingByUserIdDto?> GetActiveOrderingByUserId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetActiveOrderingByUserId/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResultOrderingByUserIdDto>(jsonData);
            if (values != null) return values;
            return null;
        }

        public async Task<bool> DeleteOrdering(int orderingId)
        {
            var response = await _httpClient.DeleteAsync($"Orderings/{orderingId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderingId, OrderStatus status)
        {
            var response = await _httpClient.PostAsync($"orderings/SetOrderStatus/{orderingId}/{(int)status}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetOrderingStatus(int orderingId,bool newStatus)
        {
            var ordering = await this.GetOrderingById(orderingId);

            UpdateOrderingDto updateOrderingDto = new UpdateOrderingDto()
            {
                OrderingId = orderingId,
                OrderDate = ordering.OrderDate,
                BillingAddressId = ordering.BillingAddressId,
                ShippingAdressId = ordering.ShippingAdressId,
                TotalPrice = ordering.TotalPrice,
                UserId = ordering.UserId,
                Status = ordering.Status
            };
            var response = await _httpClient.PutAsJsonAsync("Orderings", updateOrderingDto);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<CreateOrderingResultDto?> CreateOrdering(int billingAdressId,int shippingAdressId)
        {
            var values = await _userService.GetUserInfo();

            var basket = await _basketService.GetBasketFromDatabase();

            decimal cargoPrice = 35;
            var companies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            if (companies != null && companies.Count > 0)
            {
                cargoPrice = companies.First().CargoPrice;
            }
            decimal totalPrice = (decimal)basket.TotalPrice + cargoPrice;
           
            //CREATE ORDERING 
            CreateOrderingDto createOrderingDto = new CreateOrderingDto()
            {
                BillingAddressId = billingAdressId,
                ShippingAdressId = shippingAdressId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                UserId = values.Id
            };
            var response = await _httpClient.PostAsJsonAsync<CreateOrderingDto>("Orderings", createOrderingDto);
            if (response.IsSuccessStatusCode)
            {
                //EĞER CreateOrdering BAŞARILI İSE
                //CREATE ORDER DETAİL
                var content = await response.Content.ReadFromJsonAsync<CreateOrderingResultDto>();
                if (content != null)
                {
                    if (content.OrderingId != 0) {
                        var responseId = content.OrderingId;

                        var list = new List<CreateOrderDetailDto>();
                        foreach (var item in basket.BasketItems)
                        {
                            var orderdetail = new CreateOrderDetailDto()
                            {
                                OrderingId = responseId,
                                ProductAmount = item.Quantity,
                                ProductId = item.ProductId,
                                ProductName = item.ProductName,
                                ProductPrice = item.Price,
                                ProductTotalPrice = item.Price * item.Quantity,
                                ProductFilters = item.SelectedFilter
                            };
                            list.Add(orderdetail);
                        }
                        var response2 = await _httpClient.PostAsJsonAsync<List<CreateOrderDetailDto>>("OrderDetail", list);
                        if (response2.IsSuccessStatusCode)
                        {

                            return new CreateOrderingResultDto { OrderingId = responseId };
                        }
                        else
                        {
                            return null;
                        }
                    }else
                    {
                        return null;
                    }
                    
                    
                }else
                {
                    return null;
                }

               
            }
            return null;

        }


    }
}