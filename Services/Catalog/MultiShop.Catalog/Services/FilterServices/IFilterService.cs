using MultiShop.Catalog.DTOs.FilterDTOs;

namespace MultiShop.Catalog.Services.FilterServices
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
