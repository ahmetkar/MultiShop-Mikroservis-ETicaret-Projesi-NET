using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDTOs;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailDescriptionComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _ProductDetailDescriptionComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }



        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _httpService.GetOne<GetByIdProductDetailDto>("ProductDetails/GetProductDetailByProductId?id=" + id);
            if (result != null) return View(result);
            return View();
        }
    }
}
