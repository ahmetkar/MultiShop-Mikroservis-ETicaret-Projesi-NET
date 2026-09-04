using MultiShop.DtoLayer.CargoDtos.CargoOperationDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CargoServices.CargoOperationServices
{
    public class CargoOperationService : ICargoOperationService
    {
        private readonly HttpClient _httpClient;

        public CargoOperationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCargoOperationDto>> GetAllCargoOperationsAsync()
        {
            var response = await _httpClient.GetAsync("CargoOperations");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultCargoOperationDto>>(jsonData) ?? new List<ResultCargoOperationDto>();
            }
            return new List<ResultCargoOperationDto>();
        }

        public async Task<ResultCargoOperationDto?> GetByIdCargoOperationAsync(int id)
        {
            var response = await _httpClient.GetAsync($"CargoOperations/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResultCargoOperationDto>(jsonData);
            }
            return null;
        }

        public async Task<bool> ConfirmDeliveryAsync(int id)
        {
            var response = await _httpClient.PostAsync($"CargoOperations/ConfirmDelivery/{id}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
