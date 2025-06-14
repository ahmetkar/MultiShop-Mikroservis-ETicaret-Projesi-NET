using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _OfferDiscountDefaultViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _OfferDiscountDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.Get<ResultOfferDiscountDto>("OfferDiscounts");
            if (result != null) return View(result);
            return View();
        }
    }
}
