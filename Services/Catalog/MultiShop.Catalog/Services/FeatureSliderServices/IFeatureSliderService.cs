using MultiShop.Catalog.DTOs.FeatureSliderDTOs;

namespace MultiShop.Catalog.Services.FeatureSliderServices
{
    public interface IFeatureSliderService
    {

        Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync();
        Task CreateCatagoryAsync(CreateFeatureSliderDto createFeatureSliderDto);
        Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto);
        Task DeleteFeatureSliderAsync(string id);
        Task<GetByIdFeatureSliderDto> GetByIdFeatureSlider(string id);

        Task FeatureSliderChangeStatusToTrue(string id);
        Task FeatureSliderChangeStatusToFalse(string id);   
    }
}
