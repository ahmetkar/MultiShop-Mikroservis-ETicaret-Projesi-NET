using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultViewComponentPartial : ViewComponent
    {

        private readonly IHttpService _httpService;
        public _FeatureProductsDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.Get<ResultProductDto>("Products");
            if (result != null) return View(result);
            return View();
        }
    }
}
