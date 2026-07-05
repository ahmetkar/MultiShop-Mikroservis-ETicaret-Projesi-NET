using MultiShop.Catalog.DTOs.AboutDTOs;

namespace MultiShop.Catalog.Services.AboutServices
{
    public interface IAboutService
    {
   
        Task<bool> UpdateAboutAsync(UpdateAboutDto updateAboutDto);
        Task<ResultAboutDto> GetAbout();
    }
}
