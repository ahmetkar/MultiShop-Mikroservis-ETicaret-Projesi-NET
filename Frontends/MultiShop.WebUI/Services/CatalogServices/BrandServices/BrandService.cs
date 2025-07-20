using MultiShop.DtoLayer.CatalogDtos.BrandDtos;

namespace MultiShop.WebUI.Services.CatalogServices.BrandServices
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateBrandAsync(CreateBrandDto createBrandDto)
        {
            await _httpClient.PostAsJsonAsync<CreateBrandDto>("brands", createBrandDto);

        }

        public async Task DeleteBrandAsync(string id)
        {
            await _httpClient.DeleteAsync("brands?id=" + id);
        }

        public async Task<List<ResultBrandDto>> GetAllBrandAsync()
        {
            var resp = await _httpClient.GetAsync("brands");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultBrandDto>>();
            return values;
        }

        public async Task<UpdateBrandDto> GetByIdBrand(string id)
        {
            var resp = await _httpClient.GetAsync("brands/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateBrandDto>();
            return values;

        }

        public async Task UpdateBrandAsync(UpdateBrandDto updateBrandDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateBrandDto>("brands", updateBrandDto);

        }
    }
}
