using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CargoServices.CargoDetailServices
{
    public class CargoDetailService : ICargoDetailService
    {
        private readonly HttpClient _httpClient;

        public CargoDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCargoDetailDto>> GetAllCargoDetailsAsync()
        {
            var response = await _httpClient.GetAsync("CargoDetails");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultCargoDetailDto>>(jsonData) ?? new List<ResultCargoDetailDto>();
            }
            return new List<ResultCargoDetailDto>();
        }

        public async Task<ResultCargoDetailDto?> GetByIdCargoDetailAsync(int id)
        {
            var response = await _httpClient.GetAsync($"CargoDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResultCargoDetailDto>(jsonData);
            }
            return null;
        }
    }
}
