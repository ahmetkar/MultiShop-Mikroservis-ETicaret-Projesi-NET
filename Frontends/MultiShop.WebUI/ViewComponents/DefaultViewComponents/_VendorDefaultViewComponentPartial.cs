using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.BrandDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _VendorDefaultViewComponentPartial : ViewComponent
    {
  
        private readonly IHttpService _httpService;
        public _VendorDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.Get<ResultBrandDto>("Brands");
            if (result != null) return View(result);
            return View();
        }
    }
}
