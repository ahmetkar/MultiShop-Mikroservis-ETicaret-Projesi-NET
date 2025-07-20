using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _FooterLayoutViewComponentPartial : ViewComponent
    {
        private readonly IAboutService _aboutService;
        public _FooterLayoutViewComponentPartial(IAboutService aboutService)
        {
          _aboutService = aboutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _aboutService.GetLastAboutAsync();
            return View(result);
        }
    }
}
