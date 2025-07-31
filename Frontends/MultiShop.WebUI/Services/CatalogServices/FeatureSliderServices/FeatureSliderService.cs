using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly HttpClient _httpClient;

        public FeatureSliderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            await _httpClient.PostAsJsonAsync("featuresliders", createFeatureSliderDto);

        }

        public async Task DeleteFeatureSliderAsync(string id)
        {
            await _httpClient.DeleteAsync("featuresliders?id=" + id);
        }

        public Task FeatureSliderChangeStatusToFalse(string id)
        {
            throw new NotImplementedException();
        }

        public Task FeatureSliderChangeStatusToTrue(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
        {
            var resp = await _httpClient.GetAsync("featuresliders");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultFeatureSliderDto>>();
            return values;
        }

        public async Task<UpdateFeatureSliderDto> GetByIdFeatureSlider(string id)
        {
            var resp = await _httpClient.GetAsync("featuresliders/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateFeatureSliderDto>();
            return values;

        }

        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            await _httpClient.PutAsJsonAsync("featuresliders", updateFeatureSliderDto);

        }


    }
}
