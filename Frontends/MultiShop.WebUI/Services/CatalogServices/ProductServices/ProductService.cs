using MultiShop.DtoLayer.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        /*
         *  var values = await resp.Content.ReadFromJsonAsync<T>();
         *  alternatifi
             var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(jsonData);
         */

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync("products?id=" + id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var resp = await _httpClient.GetAsync("products");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return values;
        }

        public async Task<UpdateProductDto> GetByIdProductForUpdate(string id)
        {
            var resp = await _httpClient.GetAsync("products/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateProductDto>();
            return values;
        }

        public async Task<ResultProductDto> GetByIdProduct(string id)
        {
            var resp = await _httpClient.GetAsync("products/" + id);
            var values = await resp.Content.ReadFromJsonAsync<ResultProductDto>();
            return values;
        }

        public async Task<List<ResultProductWithCategory>> GetProductsWithCategoryAsync()
        {
            var resp = await _httpClient.GetAsync("products/ProductListWithCategory");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultProductWithCategory>>();
            return values;
        }

        public async Task<List<ResultProductWithCategory>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            //ProductListWithCategoryByCategoryId
            var resp = await _httpClient.GetAsync("products/ProductListWithCategoryByCategoryId?id="+CategoryId);
            var values = await resp.Content.ReadFromJsonAsync<List<ResultProductWithCategory>>();
            return values;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
