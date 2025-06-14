using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _FooterLayoutViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _FooterLayoutViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.GetOne<ResultAboutDto>("Abouts/GetLastAbout");
            if (result != null) return View(result);
            return View();
        }
    }
}
