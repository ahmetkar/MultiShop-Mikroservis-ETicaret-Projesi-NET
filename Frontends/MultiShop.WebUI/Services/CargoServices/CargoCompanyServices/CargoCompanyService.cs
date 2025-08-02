using MultiShop.DtoLayer.CargoDtos.CargoCompanyDtos;

namespace MultiShop.WebUI.Services.CargoServices.CargoCompanyServices
{
    public class CargoCompanyService : ICargoCompanyService
    {
        private readonly HttpClient _httpClient;

        public CargoCompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCargoCompanyAsync(CreateCargoCompanyDto createCargoCompanyDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCargoCompanyDto>("CargoCompanies", createCargoCompanyDto);

        }

        public async Task DeleteCargoCompanyAsync(int id)
        {
            await _httpClient.DeleteAsync("CargoCompanies?id=" + id);
        }

        public async Task<List<ResultCargoCompanyDto>> GetAllCargoCompanyAsync()
        {
            var resp = await _httpClient.GetAsync("CargoCompanies");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultCargoCompanyDto>>();
            return values;
        }

        public async Task<UpdateCargoCompanyDto> GetByIdCargoCompany(int id)
        {
            var resp = await _httpClient.GetAsync("CargoCompanies/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateCargoCompanyDto>();
            return values;
        }


        public async Task UpdateCargoCompanyAsync(UpdateCargoCompanyDto updateCargoCompanyDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCargoCompanyDto>("CargoCompanies", updateCargoCompanyDto);
        }
    }
}
