using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListPaginationComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListPaginationComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryid, List<string>? filterIds, int currentPage = 1)
        {
            int pageSize = 9;
            var totalCount = await _productService.GetProductCountByCategoryAndFiltersAsync(categoryid, filterIds);
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.CategoryId = categoryid;
            ViewBag.FilterIds = filterIds ?? new List<string>();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages > 0 ? totalPages : 1;

            return View();
        }
    }
}
