using MultiShop.DtoLayer.CatalogDtos.ProductFilterDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductFilterServices
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly HttpClient _httpClient;

        public ProductFilterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AssignFiltersToCategoryAsync(CategoryFilterAssignDto assignDto)
        {
            await _httpClient.PostAsJsonAsync("productfilters/AssignFiltersToCategory", assignDto);
        }

        public async Task CreateProductFilterAsync(CreateProductFilterDto createProductFilterDto)
        {
            await _httpClient.PostAsJsonAsync("productfilters", createProductFilterDto);
        }

        public async Task DeleteProductFilterAsync(string id)
        {
            await _httpClient.DeleteAsync("productfilters/" + id);
        }

        public async Task<List<ResultProductFilterDto>> GetAllProductFilterAsync()
        {
            var response = await _httpClient.GetAsync("productfilters");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultProductFilterDto>>() ?? new List<ResultProductFilterDto>();
            }
            return new List<ResultProductFilterDto>();
        }

        public async Task<GetByIdProductFilterDto> GetByIdProductFilterAsync(string id)
        {
            var response = await _httpClient.GetAsync("productfilters/" + id);
            return await response.Content.ReadFromJsonAsync<GetByIdProductFilterDto>();
        }

        public async Task<List<ResultProductFilterDto>> GetProductFiltersByCategoryIdAsync(string categoryId)
        {
            var response = await _httpClient.GetAsync("productfilters/GetProductFiltersByCategoryId/" + categoryId);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultProductFilterDto>>() ?? new List<ResultProductFilterDto>();
            }
            return new List<ResultProductFilterDto>();
        }

        public async Task UpdateProductFilterAsync(UpdateProductFilterDto updateProductFilterDto)
        {
            await _httpClient.PutAsJsonAsync("productfilters", updateProductFilterDto);
        }
    }
}
