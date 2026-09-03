using MultiShop.DtoLayer.CargoDtos.CargoCustotmerDtos;
using System.Net.Http.Json;

namespace MultiShop.WebUI.Services.CargoServices.CargoCustomerServices
{
    public class CargoCustomerService : ICargoCustomerService
    {
        private readonly HttpClient _httpClient;

        public CargoCustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }   

        public async Task<GetCargoCustomerByIdDto?> GetByIdCargoCustomerInfoAsync(string id)
        {
            var resp = await _httpClient.GetAsync("CargoCustomers/GetCargoCustomerById?id=" + id);
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<GetCargoCustomerByIdDto>();
                return values;
            }
            return null;
        }
    }
}
