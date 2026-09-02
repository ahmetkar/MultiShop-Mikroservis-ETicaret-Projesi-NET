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

        public async Task<List<ResultProductWithCategory>> GetProductsByCategoryAndFiltersAsync(string categoryId, List<string>? filterIds, int page = 1, int pageSize = 9)
        {
            var url = $"products/GetProductsByCategoryAndFilters?categoryId={categoryId}&page={page}&pageSize={pageSize}";
            if (filterIds != null && filterIds.Count > 0)
            {
                foreach (var fId in filterIds)
                {
                    url += $"&filterIds={fId}";
                }
            }
            var resp = await _httpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<List<ResultProductWithCategory>>() ?? new List<ResultProductWithCategory>();
            }
            return new List<ResultProductWithCategory>();
        }

        public async Task<long> GetProductCountByCategoryAndFiltersAsync(string categoryId, List<string>? filterIds)
        {
            var url = $"products/GetProductCountByCategoryAndFilters?categoryId={categoryId}";
            if (filterIds != null && filterIds.Count > 0)
            {
                foreach (var fId in filterIds)
                {
                    url += $"&filterIds={fId}";
                }
            }
            var resp = await _httpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<long>();
            }
            return 0;
        }

        public async Task<List<ResultProductWithCategory>> GetLast20ProductsAsync()
        {
            var resp = await _httpClient.GetAsync("products/GetLast20Products");
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<List<ResultProductWithCategory>>() ?? new List<ResultProductWithCategory>();
            }
            return new List<ResultProductWithCategory>();
        }

        public async Task<List<ResultProductWithCategory>> SearchProductsAsync(string query, int page = 1, int pageSize = 9)
        {
            var url = $"products/SearchProducts?query={Uri.EscapeDataString(query ?? "")}&page={page}&pageSize={pageSize}";
            var resp = await _httpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<List<ResultProductWithCategory>>() ?? new List<ResultProductWithCategory>();
            }
            return new List<ResultProductWithCategory>();
        }

        public async Task<long> GetSearchProductCountAsync(string query)
        {
            var url = $"products/GetSearchProductCount?query={Uri.EscapeDataString(query ?? "")}";
            var resp = await _httpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<long>();
            }
            return 0;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
