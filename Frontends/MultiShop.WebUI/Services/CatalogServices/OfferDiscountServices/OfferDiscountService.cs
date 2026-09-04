using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;

namespace MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices
{
    public class OfferDiscountService : IOfferDiscountService
    {
        private readonly HttpClient _httpClient;

        public OfferDiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateOfferDiscountAsync(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _httpClient.PostAsJsonAsync<CreateOfferDiscountDto>("offerdiscounts", createOfferDiscountDto);

        }

        public async Task DeleteOfferDiscountAsync(string id)
        {
            await _httpClient.DeleteAsync("offerdiscounts?id=" + id);
        }

        public async Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync()
        {
            var resp = await _httpClient.GetAsync("offerdiscounts");
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<List<ResultOfferDiscountDto>>();
                return values ?? new List<ResultOfferDiscountDto>();
            }
            return new List<ResultOfferDiscountDto>();
        }

        public async Task<UpdateOfferDiscountDto> GetByIdOfferDiscount(string id)
        {
            var resp = await _httpClient.GetAsync("offerdiscounts/" + id);
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<UpdateOfferDiscountDto>();
                return values!;
            }
            return new UpdateOfferDiscountDto();
        }

        public async Task UpdateOfferDiscountAsync(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateOfferDiscountDto>("offerdiscounts", updateOfferDiscountDto);

        }
    }
}
