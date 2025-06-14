using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDTOs;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailImageSliderComponentPartial : ViewComponent
    {

        private readonly IHttpService _httpService;
        public _ProductDetailImageSliderComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }



        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _httpService.GetOne<GetByIdProductImageDto>("ProductImages/ProductImagesByProductId?id=" + id);
            if (result != null) return View(result);
            return View();
        }
    }
}
