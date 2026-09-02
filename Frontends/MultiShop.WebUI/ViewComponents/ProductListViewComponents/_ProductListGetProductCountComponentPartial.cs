using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListGetProductCountComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListGetProductCountComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryid, List<string>? filterIds)
        {
            var count = await _productService.GetProductCountByCategoryAndFiltersAsync(categoryid, filterIds);
            ViewBag.ProductCount = count;
            return View();
        }
    }
}
