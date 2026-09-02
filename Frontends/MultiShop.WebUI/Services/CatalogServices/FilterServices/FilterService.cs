using MultiShop.DtoLayer.CatalogDtos.FilterDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FilterServices
{
    public class FilterService : IFilterService
    {
        private readonly HttpClient _httpClient;

        public FilterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateFilterAsync(CreateFilterDto createFilterDto)
        {
            await _httpClient.PostAsJsonAsync("filters", createFilterDto);
        }

        public async Task DeleteFilterAsync(string id)
        {
            await _httpClient.DeleteAsync("filters/" + id);
        }

        public async Task<List<ResultFilterDto>> GetAllFilterAsync()
        {
            var response = await _httpClient.GetAsync("filters");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultFilterDto>>() ?? new List<ResultFilterDto>();
            }
            return new List<ResultFilterDto>();
        }

        public async Task<GetByIdFilterDto> GetByIdFilterAsync(string id)
        {
            var response = await _httpClient.GetAsync("filters/" + id);
            return await response.Content.ReadFromJsonAsync<GetByIdFilterDto>();
        }

        public async Task UpdateFilterAsync(UpdateFilterDto updateFilterDto)
        {
            await _httpClient.PutAsJsonAsync("filters", updateFilterDto);
        }
    }
}
