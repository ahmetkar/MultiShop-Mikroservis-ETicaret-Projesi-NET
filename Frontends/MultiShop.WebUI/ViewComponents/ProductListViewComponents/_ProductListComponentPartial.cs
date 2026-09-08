using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly IFilterService _filterService;

        public _ProductListComponentPartial(IProductService productService, IDiscountService discountService, IFilterService filterService)
        {
            _productService = productService;
            _discountService = discountService;
            _filterService = filterService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryid, List<string>? filterIds, int page = 1)
        {
            var result = await _productService.GetProductsByCategoryAndFiltersAsync(categoryid, filterIds, page, 9);
            var discounts = await _discountService.GetActiveProductDiscountsAsync();
            var discountDict = discounts.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.First().Rate);
            ViewBag.DiscountDict = discountDict;

            var allFilters = await _filterService.GetAllFilterAsync();
            ViewBag.AllFilters = allFilters;

            return View(result);
        }   
    }
}
