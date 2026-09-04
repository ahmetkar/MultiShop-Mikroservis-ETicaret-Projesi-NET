using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.OrderServices.OrderDetailServices
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly HttpClient _httpClient;

        public OrderDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingId(int orderingId)
        {
            var response = await _httpClient.GetAsync($"OrderDetail/{orderingId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultOrderDetailDto>>(jsonData) ?? new List<ResultOrderDetailDto>();
            }
            return new List<ResultOrderDetailDto>();
        }

        public async Task<List<ResultOrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var response = await _httpClient.GetAsync("OrderDetail");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultOrderDetailDto>>(jsonData) ?? new List<ResultOrderDetailDto>();
            }
            return new List<ResultOrderDetailDto>();
        }
    }
}

