using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IFilterService _filterService;
        private readonly IDiscountService _discountService;

        public ProductController(IProductService productService, ICategoryService categoryService, IFilterService filterService, IDiscountService discountService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _filterService = filterService;
            _discountService = discountService;
        }

        void ProductViewBags(string pagename)
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = pagename;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ProductViewBags("Ürün Listesi");

            var values = await _productService.GetAllProductAsync();
            return View(values);
        }


        [Route("ProductListWithCategory")]
        public async Task<IActionResult> ProductListWithCategory()
        {
            ProductViewBags("Ürün Listesi");

            var result = await _productService.GetProductsWithCategoryAsync();
            if (result != null) return View(result);
            return View();
        }

        [HttpGet]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ProductViewBags("Ürün Ekle");

            var catresults = await _categoryService.GetAllCategoryAsync();
            if (catresults != null)
            {
                List<SelectListItem> categoryValues = (from c in catresults select new SelectListItem { Text = c.CategoryName, Value = c.CategoryID }).ToList();
                ViewBag.Categories = categoryValues;
            }
            else
            {
                ViewBag.Categories = null;
            }

            var filters = await _filterService.GetAllFilterAsync();
            ViewBag.AllFilters = filters;

            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto, List<string>? selectedFilterIds, int? discountRate, DateTime? discountValidDate, bool? isDiscountActive)
        {
            decimal kdvpercent = createProductDto.KDVPercent;
            createProductDto.KDVPrice = (createProductDto.ProductPrice * kdvpercent) / 100;
            createProductDto.FilterIds = selectedFilterIds ?? new List<string>();

            if (string.IsNullOrEmpty(createProductDto.ProductId))
            {
                createProductDto.ProductId = Guid.NewGuid().ToString("N").Substring(0, 24);
            }

            await _productService.CreateProductAsync(createProductDto);

            if (discountRate.HasValue && discountRate.Value > 0)
            {
                var validDate = discountValidDate ?? DateTime.Now.AddDays(30);
                var active = isDiscountActive ?? true;
                await _discountService.SetProductDiscountAsync(createProductDto.ProductId, discountRate.Value, validDate, active);
            }

            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }

        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            try
            {
                await _discountService.DeleteDiscountByProductIdAsync(id);
            }
            catch { }
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }

        [Route("UpdateProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ProductViewBags("Ürün Güncelle");

            var catresults = await _categoryService.GetAllCategoryAsync();
            if (catresults != null)
            {
                List<SelectListItem> categoryValues = (from c in catresults select new SelectListItem { Text = c.CategoryName, Value = c.CategoryID }).ToList();
                ViewBag.Categories = categoryValues;
            }
            else
            {
                ViewBag.Categories = null;
            }

            var filters = await _filterService.GetAllFilterAsync();
            ViewBag.AllFilters = filters;

            var existingDiscount = await _discountService.GetDiscountByProductIdAsync(id);
            ViewBag.DiscountRate = existingDiscount?.Rate ?? 0;
            ViewBag.DiscountValidDate = existingDiscount?.ValidDate.ToString("yyyy-MM-ddTHH:mm") ?? DateTime.Now.AddDays(30).ToString("yyyy-MM-ddTHH:mm");
            ViewBag.IsDiscountActive = existingDiscount?.IsActive ?? false;

            var result = await _productService.GetByIdProductForUpdate(id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateProduct/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto, List<string>? selectedFilterIds, int? discountRate, DateTime? discountValidDate, bool? isDiscountActive)
        {
            decimal kdvpercent = updateProductDto.KDVPercent;
            updateProductDto.KDVPrice = (updateProductDto.ProductPrice * kdvpercent) / 100;
            updateProductDto.FilterIds = selectedFilterIds ?? new List<string>();
            await _productService.UpdateProductAsync(updateProductDto);

            if (discountRate.HasValue && discountRate.Value > 0)
            {
                var validDate = discountValidDate ?? DateTime.Now.AddDays(30);
                var active = isDiscountActive ?? true;
                await _discountService.SetProductDiscountAsync(updateProductDto.ProductId, discountRate.Value, validDate, active);
            }
            else
            {
                await _discountService.DeleteDiscountByProductIdAsync(updateProductDto.ProductId);
            }

            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
    }
}
    