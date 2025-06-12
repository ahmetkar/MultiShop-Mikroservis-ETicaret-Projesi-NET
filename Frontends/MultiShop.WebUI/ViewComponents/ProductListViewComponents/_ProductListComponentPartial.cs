using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _ProductListComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            //id = "6826201453278247d82935f6";
            var result = await _httpService.Get<ResultProductWithCategory>("Products/ProductListWithCategoryByCategoryId?id=" + id);
            if (result != null) return View(result);
            
            return View();
        }   
    }
}
