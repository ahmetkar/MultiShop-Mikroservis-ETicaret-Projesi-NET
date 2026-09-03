using MultiShop.Catalog.DTOs.ProductDTOs;

namespace MultiShop.Catalog.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByIdProduct(string id);
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync();
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId);
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAndFiltersAsync(string CategoryId, List<string>? filterIds, int page = 1, int pageSize = 9);
        Task<long> GetProductCountByCategoryIdAndFiltersAsync(string CategoryId, List<string>? filterIds);
        Task<List<ResultProductsWithCategoryDto>> GetLast20ProductsAsync();
        Task<List<ResultProductsWithCategoryDto>> SearchProductsAsync(string query, int page = 1, int pageSize = 9);
        Task<long> GetSearchProductCountAsync(string query);
        Task<List<ResultProductsWithCategoryDto>> GetProductsByIdsAsync(List<string> productIds);
    }
}

