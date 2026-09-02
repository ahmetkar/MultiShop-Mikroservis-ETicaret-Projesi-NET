using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductFilterDtos;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductFilterServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductsFilterListComponentPartial : ViewComponent
    {
        private readonly IProductFilterService _productFilterService;
        private readonly IFilterService _filterService;

        public _ProductsFilterListComponentPartial(IProductFilterService productFilterService, IFilterService filterService)
        {
            _productFilterService = productFilterService;
            _filterService = filterService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryid, List<string>? selectedFilterIds)
        {
            var productFilters = await _productFilterService.GetProductFiltersByCategoryIdAsync(categoryid);
            ViewBag.CategoryId = categoryid;
            ViewBag.SelectedFilterIds = selectedFilterIds ?? new List<string>();

            return View(productFilters);
        }
    }
}
