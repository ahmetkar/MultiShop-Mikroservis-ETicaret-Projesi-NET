using MultiShop.DtoLayer.CatalogDtos.ProductFilterDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductFilterServices
{
    public interface IProductFilterService
    {
        Task<List<ResultProductFilterDto>> GetAllProductFilterAsync();
        Task<List<ResultProductFilterDto>> GetProductFiltersByCategoryIdAsync(string categoryId);
        Task AssignFiltersToCategoryAsync(CategoryFilterAssignDto assignDto);
        Task CreateProductFilterAsync(CreateProductFilterDto createProductFilterDto);
        Task UpdateProductFilterAsync(UpdateProductFilterDto updateProductFilterDto);
        Task DeleteProductFilterAsync(string id);
        Task<GetByIdProductFilterDto> GetByIdProductFilterAsync(string id);
    }
}
