using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _CarouselDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


    
        public async  Task<IViewComponentResult> InvokeAsync()
        {

            var result = await _httpService.Get<ResultFeatureSliderDto>("FeatureSliders");
            if (result != null) return View(result);
            return View();
        }
    }
}
