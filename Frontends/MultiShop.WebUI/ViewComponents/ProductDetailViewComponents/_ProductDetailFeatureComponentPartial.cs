using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailFeatureComponentPartial : ViewComponent
    {
        

        private readonly IHttpService _httpService;
        public _ProductDetailFeatureComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }



        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _httpService.GetOne<ResultProductDto>("Products/"+id);
            if (result != null) return View(result);
            return View();
        }
    }
}
