using MultiShop.DtoLayer.CatalogDtos.ProductDetailDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ProductDetailServices
{
    public interface IProductDetailService
    {
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();
        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);
        Task DeleteProductDetailAsync(string id);
        Task<GetByIdProductDetailDto> GetByIdProductDetail(string id);
        Task<UpdateProductDetailDto> GetByProductIdProductDetailForUpdate(string id);
        Task<GetByIdProductDetailDto> GetByProductIdProductDetail(string id);
    }
}
