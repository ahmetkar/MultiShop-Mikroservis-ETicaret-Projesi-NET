using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices
{
    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly HttpClient _httpClient;

        public SpecialOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto)
        {
            await _httpClient.PostAsJsonAsync<CreateSpecialOfferDto>("specialoffers", createSpecialOfferDto);

        }   

        public async Task DeleteSpecialOfferAsync(string id)
        {
            await _httpClient.DeleteAsync("specialoffers?id=" + id);
        }

        public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync()
        {
            var resp = await _httpClient.GetAsync("specialoffers");
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<List<ResultSpecialOfferDto>>();
                return values ?? new List<ResultSpecialOfferDto>();
            }
            return new List<ResultSpecialOfferDto>();
        }

        public async Task<UpdateSpecialOfferDto> GetByIdSpecialOffer(string id)
        {
            var resp = await _httpClient.GetAsync("specialoffers/" + id);
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<UpdateSpecialOfferDto>();
                return values!;
            }
            return new UpdateSpecialOfferDto();
        }

        public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateSpecialOfferDto>("specialoffers", updateSpecialOfferDto);

        }
    }
}
