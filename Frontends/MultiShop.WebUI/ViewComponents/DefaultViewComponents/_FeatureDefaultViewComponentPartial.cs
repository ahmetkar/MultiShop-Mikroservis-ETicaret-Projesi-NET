using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureDTOs;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureDefaultViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _FeatureDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;
          
            _httpService.setUrl("CatalogApi");
        }



        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.Get<ResultFeatureDto>("Features");
            if (result != null) return View(result);
            return View();
        }
    }
}
