using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Areas.Admin.Services;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _CarouselDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("http://localhost:7070/api/");
        }


    
        public async  Task<IViewComponentResult> InvokeAsync()
        {

            var result = await _httpService.Get<ResultFeatureSliderDto>("FeatureSliders");
            if (result != null) return View(result);
            return View();
        }
    }
}
