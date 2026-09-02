using MultiShop.DtoLayer.CatalogDtos.FilterDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FilterServices
{
    public interface IFilterService
    {
        Task<List<ResultFilterDto>> GetAllFilterAsync();
        Task CreateFilterAsync(CreateFilterDto createFilterDto);
        Task UpdateFilterAsync(UpdateFilterDto updateFilterDto);
        Task DeleteFilterAsync(string id);
        Task<GetByIdFilterDto> GetByIdFilterAsync(string id);
    }
}
