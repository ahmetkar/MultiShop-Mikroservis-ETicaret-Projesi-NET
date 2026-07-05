using MultiShop.DtoLayer.CatalogDtos.AboutDtos;

namespace MultiShop.WebUI.Services.CatalogServices.AboutServices
{
    public interface IAboutService
    {
  
        Task UpdateAboutAsync(UpdateAboutDto updateAboutDto);
        Task<ResultAboutDto> GetAbout();
      
    }
}
