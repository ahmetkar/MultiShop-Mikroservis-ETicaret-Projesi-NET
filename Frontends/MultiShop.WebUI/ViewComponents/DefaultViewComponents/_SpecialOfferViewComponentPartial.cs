using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDTOs;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _SpecialOfferViewComponentPartial : ViewComponent
    {


        private readonly IHttpService _httpService;
        public _SpecialOfferViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            var result = await _httpService.Get<ResultSpecialOfferDto>("SpecialOffers");
            if (result != null) return View(result);
            return View();
        }
    }

}