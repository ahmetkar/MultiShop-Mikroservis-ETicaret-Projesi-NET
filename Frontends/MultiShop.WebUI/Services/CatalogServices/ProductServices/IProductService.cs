using MultiShop.DtoLayer.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<UpdateProductDto> GetByIdProduct(string id);
        Task<List<ResultProductWithCategory>> GetProductsWithCategoryAsync();
        Task<List<ResultProductWithCategory>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId);
    }
}
