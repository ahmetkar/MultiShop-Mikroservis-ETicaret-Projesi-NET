using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FilterDtos;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailFeatureComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IFilterService _filterService;
        private readonly IDiscountService _discountService;

        public _ProductDetailFeatureComponentPartial(IProductService productService, IFilterService filterService, IDiscountService discountService)
        {
            _productService = productService;
            _filterService = filterService;
            _discountService = discountService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _productService.GetByIdProduct(id);
            var allFilters = await _filterService.GetAllFilterAsync();

            var productFilters = new List<ResultFilterDto>();
            if (result != null && result.FilterIds != null && result.FilterIds.Count > 0)
            {
                productFilters = allFilters.Where(f => result.FilterIds.Contains(f.FilterId)).ToList();
            }
            ViewBag.ProductFilters = productFilters;

            var discount = await _discountService.GetDiscountByProductIdAsync(id);
            ViewBag.DiscountRate = discount?.Rate ?? 0;

            return View(result);
        }
    }
}
