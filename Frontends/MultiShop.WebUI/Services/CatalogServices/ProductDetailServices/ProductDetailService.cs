using MultiShop.DtoLayer.CatalogDtos.ProductDetailDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ProductDetailServices
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly HttpClient _httpClient;

        public ProductDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductDetailDto>("productdetails", createProductDetailDto);

        }

        public async Task DeleteProductDetailAsync(string id)
        {
            await _httpClient.DeleteAsync("productdetails?id=" + id);
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
        {
            var resp = await _httpClient.GetAsync("productdetails");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultProductDetailDto>>();
            return values;
        }

        public async Task<GetByIdProductDetailDto> GetByIdProductDetail(string id)
        {
            var resp = await _httpClient.GetAsync("productdetails/" + id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdProductDetailDto>();
            return values;

        }

        public async Task<UpdateProductDetailDto> GetByProductIdProductDetailForUpdate(string id)
        {
            var resp = await _httpClient.GetAsync("productdetails/GetProductDetailByProductId?id=" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateProductDetailDto>();
            return values;
        }

        public async Task<GetByIdProductDetailDto> GetByProductIdProductDetail(string id)
        {
            var resp = await _httpClient.GetAsync("productdetails/GetProductDetailByProductId?id=" + id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdProductDetailDto>();
            return values;
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDetailDto>("productdetails", updateProductDetailDto);

        }
    }
}
