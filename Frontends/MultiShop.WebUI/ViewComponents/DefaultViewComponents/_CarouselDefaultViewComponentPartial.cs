using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultViewComponentPartial : ViewComponent
    {

        private readonly IFeatureSliderService _featureSliderService;
        public _CarouselDefaultViewComponentPartial(IFeatureSliderService featureSliderService)
        {
           _featureSliderService = featureSliderService;
        }

        public async  Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _featureSliderService.GetAllFeatureSliderAsync();
            return View(result);
        }
    }
}
