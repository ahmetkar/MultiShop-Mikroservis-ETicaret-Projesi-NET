using MultiShop.DtoLayer.CatalogDtos.ProductImageDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductImageDto>("productimages", createProductImageDto);

        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _httpClient.DeleteAsync("productimages?id=" + id);
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            var resp = await _httpClient.GetAsync("productimages");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultProductImageDto>>();
            return values;
        }

        public async Task<GetByIdProductImageDto> GetByIdProductImage(string id)
        {
            var resp = await _httpClient.GetAsync("productimages" + id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdProductImageDto>();
            return values;

        }

        public async Task<GetByIdProductImageDto> GetByProductIdProductImagesAsync(string id)
        {
            var resp = await _httpClient.GetAsync("productimages/ProductImagesByProductId?id=" + id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdProductImageDto>();
            return values;
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductImageDto>("productimages", updateProductImageDto);

        }
    }
}
