using MultiShop.DtoLayer.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<UpdateProductDto> GetByIdProductForUpdate(string id);
        Task<ResultProductDto> GetByIdProduct(string id);
        Task<List<ResultProductWithCategory>> GetProductsWithCategoryAsync();
        Task<List<ResultProductWithCategory>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId);
        Task<List<ResultProductWithCategory>> GetProductsByCategoryAndFiltersAsync(string categoryId, List<string>? filterIds, int page = 1, int pageSize = 9);
        Task<long> GetProductCountByCategoryAndFiltersAsync(string categoryId, List<string>? filterIds);
        Task<List<ResultProductWithCategory>> GetLast20ProductsAsync();
        Task<List<ResultProductWithCategory>> SearchProductsAsync(string query, int page = 1, int pageSize = 9);
        Task<long> GetSearchProductCountAsync(string query);
    }
}
