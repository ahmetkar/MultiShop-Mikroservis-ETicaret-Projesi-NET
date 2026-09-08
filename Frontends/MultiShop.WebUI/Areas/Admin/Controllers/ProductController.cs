using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDTOs;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductImageServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;
using MultiShop.WebUI.Services.FileUploadServices;

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
        private readonly IProductImageService _productImageService;
        private readonly IFileUploadService _fileUploadService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IFilterService filterService,
            IDiscountService discountService,
            IProductImageService productImageService,
            IFileUploadService fileUploadService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _filterService = filterService;
            _discountService = discountService;
            _productImageService = productImageService;
            _fileUploadService = fileUploadService;
        }

        void ProductViewBags(string pagename)
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = pagename;
        }
        private async Task LoadExistingImagesToViewBag()
        {
            var existingImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var products = await _productService.GetAllProductAsync();
                if (products != null)
                {
                    foreach (var p in products)
                    {
                        if (!string.IsNullOrWhiteSpace(p.ProductImageUrl))
                            existingImages.Add(p.ProductImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var productImages = await _productImageService.GetAllProductImageAsync();
                if (productImages != null)
                {
                    foreach (var pi in productImages)
                    {
                        if (!string.IsNullOrWhiteSpace(pi.Image1)) existingImages.Add(pi.Image1.Trim());
                        if (!string.IsNullOrWhiteSpace(pi.Image2)) existingImages.Add(pi.Image2.Trim());
                        if (!string.IsNullOrWhiteSpace(pi.Image3)) existingImages.Add(pi.Image3.Trim());
                        if (!string.IsNullOrWhiteSpace(pi.Image4)) existingImages.Add(pi.Image4.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/products/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
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

            await LoadExistingImagesToViewBag();

            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(
            CreateProductDto createProductDto,
            IFormFile? productImageFile,
            IFormFile? image1File,
            IFormFile? image2File,
            IFormFile? image3File,
            IFormFile? image4File,
            string? image1Url,
            string? image2Url,
            string? image3Url,
            string? image4Url,
            List<string>? selectedFilterIds,
            Dictionary<string, int>? filterQuantities,
            int? discountRate,
            DateTime? discountValidDate,
            bool? isDiscountActive)
        {
            if (productImageFile != null && productImageFile.Length > 0)
            {
                var uploadedMain = await _fileUploadService.UploadFileAsync(productImageFile, "products");
                if (!string.IsNullOrEmpty(uploadedMain))
                {
                    createProductDto.ProductImageUrl = uploadedMain;
                }
            }

            decimal kdvpercent = createProductDto.KDVPercent;
            createProductDto.KDVPrice = (createProductDto.ProductPrice * kdvpercent) / 100;
            createProductDto.FilterIds = selectedFilterIds ?? new List<string>();
            createProductDto.FilterStocks = new Dictionary<string, int>();

            if (selectedFilterIds != null && selectedFilterIds.Count > 0)
            {
                foreach (var fId in selectedFilterIds)
                {
                    int qty = 1;
                    if (filterQuantities != null && filterQuantities.TryGetValue(fId, out int q) && q > 0)
                    {
                        qty = q;
                    }
                    else if (int.TryParse(Request.Form[$"filterQuantities[{fId}]"], out int formQ) && formQ > 0)
                    {
                        qty = formQ;
                    }
                    else if (int.TryParse(Request.Form[$"filterQuantities_{fId}"], out int formQ2) && formQ2 > 0)
                    {
                        qty = formQ2;
                    }

                    createProductDto.FilterStocks[fId] = qty;
                }
            }

            if (string.IsNullOrEmpty(createProductDto.ProductId))
            {
                createProductDto.ProductId = Guid.NewGuid().ToString("N").Substring(0, 24);
            }

            await _productService.CreateProductAsync(createProductDto);

            // Multi-angle images
            string img1 = !string.IsNullOrEmpty(image1Url) ? image1Url : (createProductDto.ProductImageUrl ?? "");
            string img2 = image2Url ?? "";
            string img3 = image3Url ?? "";
            string img4 = image4Url ?? "";

            if (image1File != null && image1File.Length > 0)
            {
                var up1 = await _fileUploadService.UploadFileAsync(image1File, "products");
                if (!string.IsNullOrEmpty(up1)) img1 = up1;
            }
            if (image2File != null && image2File.Length > 0)
            {
                var up2 = await _fileUploadService.UploadFileAsync(image2File, "products");
                if (!string.IsNullOrEmpty(up2)) img2 = up2;
            }
            if (image3File != null && image3File.Length > 0)
            {
                var up3 = await _fileUploadService.UploadFileAsync(image3File, "products");
                if (!string.IsNullOrEmpty(up3)) img3 = up3;
            }
            if (image4File != null && image4File.Length > 0)
            {
                var up4 = await _fileUploadService.UploadFileAsync(image4File, "products");
                if (!string.IsNullOrEmpty(up4)) img4 = up4;
            }

            if (!string.IsNullOrEmpty(img1) || !string.IsNullOrEmpty(img2) || !string.IsNullOrEmpty(img3) || !string.IsNullOrEmpty(img4))
            {
                var galleryDto = new CreateProductImageDto
                {
                    ProductId = createProductDto.ProductId,
                    Image1 = img1,
                    Image2 = img2,
                    Image3 = img3,
                    Image4 = img4
                };
                try
                {
                    await _productImageService.CreateProductImageAsync(galleryDto);
                }
                catch { }
            }

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
            try
            {
                var existingImg = await _productImageService.GetByProductIdProductImagesAsync(id);
                if (existingImg != null && !string.IsNullOrEmpty(existingImg.ProductImageID))
                {
                    await _productImageService.DeleteProductImageAsync(existingImg.ProductImageID);
                }
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

            try
            {
                var existingImages = await _productImageService.GetByProductIdProductImagesAsync(id);
                ViewBag.ProductImages = existingImages;
            }
            catch
            {
                ViewBag.ProductImages = null;
            }

            var result = await _productService.GetByIdProductForUpdate(id);
            await LoadExistingImagesToViewBag();
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateProduct/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(
            UpdateProductDto updateProductDto,
            IFormFile? productImageFile,
            IFormFile? image1File,
            IFormFile? image2File,
            IFormFile? image3File,
            IFormFile? image4File,
            string? image1Url,
            string? image2Url,
            string? image3Url,
            string? image4Url,
            List<string>? selectedFilterIds,
            Dictionary<string, int>? filterQuantities,
            int? discountRate,
            DateTime? discountValidDate,
            bool? isDiscountActive)
        {
            if (productImageFile != null && productImageFile.Length > 0)
            {
                var uploadedMain = await _fileUploadService.UploadFileAsync(productImageFile, "products");
                if (!string.IsNullOrEmpty(uploadedMain))
                {
                    updateProductDto.ProductImageUrl = uploadedMain;
                }
            }

            decimal kdvpercent = updateProductDto.KDVPercent;
            updateProductDto.KDVPrice = (updateProductDto.ProductPrice * kdvpercent) / 100;
            updateProductDto.FilterIds = selectedFilterIds ?? new List<string>();
            updateProductDto.FilterStocks = new Dictionary<string, int>();

            if (selectedFilterIds != null && selectedFilterIds.Count > 0)
            {
                foreach (var fId in selectedFilterIds)
                {
                    int qty = 1;
                    if (filterQuantities != null && filterQuantities.TryGetValue(fId, out int q) && q > 0)
                    {
                        qty = q;
                    }
                    else if (int.TryParse(Request.Form[$"filterQuantities[{fId}]"], out int formQ) && formQ > 0)
                    {
                        qty = formQ;
                    }
                    else if (int.TryParse(Request.Form[$"filterQuantities_{fId}"], out int formQ2) && formQ2 > 0)
                    {
                        qty = formQ2;
                    }

                    updateProductDto.FilterStocks[fId] = qty;
                }
            }

            await _productService.UpdateProductAsync(updateProductDto);

            // Handle multi-angle gallery images
            GetByIdProductImageDto? existingImg = null;
            try
            {
                existingImg = await _productImageService.GetByProductIdProductImagesAsync(updateProductDto.ProductId);
            }
            catch { }

            string img1 = !string.IsNullOrEmpty(image1Url) ? image1Url : (existingImg?.Image1 ?? updateProductDto.ProductImageUrl ?? "");
            string img2 = !string.IsNullOrEmpty(image2Url) ? image2Url : (existingImg?.Image2 ?? "");
            string img3 = !string.IsNullOrEmpty(image3Url) ? image3Url : (existingImg?.Image3 ?? "");
            string img4 = !string.IsNullOrEmpty(image4Url) ? image4Url : (existingImg?.Image4 ?? "");

            if (image1File != null && image1File.Length > 0)
            {
                var up1 = await _fileUploadService.UploadFileAsync(image1File, "products");
                if (!string.IsNullOrEmpty(up1)) img1 = up1;
            }
            if (image2File != null && image2File.Length > 0)
            {
                var up2 = await _fileUploadService.UploadFileAsync(image2File, "products");
                if (!string.IsNullOrEmpty(up2)) img2 = up2;
            }
            if (image3File != null && image3File.Length > 0)
            {
                var up3 = await _fileUploadService.UploadFileAsync(image3File, "products");
                if (!string.IsNullOrEmpty(up3)) img3 = up3;
            }
            if (image4File != null && image4File.Length > 0)
            {
                var up4 = await _fileUploadService.UploadFileAsync(image4File, "products");
                if (!string.IsNullOrEmpty(up4)) img4 = up4;
            }

            if (existingImg != null && !string.IsNullOrEmpty(existingImg.ProductImageID))
            {
                var updateImgDto = new UpdateProductImageDto
                {
                    ProductImageID = existingImg.ProductImageID,
                    ProductId = updateProductDto.ProductId,
                    Image1 = img1,
                    Image2 = img2,
                    Image3 = img3,
                    Image4 = img4
                };
                try
                {
                    await _productImageService.UpdateProductImageAsync(updateImgDto);
                }
                catch { }
            }
            else if (!string.IsNullOrEmpty(img1) || !string.IsNullOrEmpty(img2) || !string.IsNullOrEmpty(img3) || !string.IsNullOrEmpty(img4))
            {
                var createImgDto = new CreateProductImageDto
                {
                    ProductId = updateProductDto.ProductId,
                    Image1 = img1,
                    Image2 = img2,
                    Image3 = img3,
                    Image4 = img4
                };
                try
                {
                    await _productImageService.CreateProductImageAsync(createImgDto);
                }
                catch { }
            }

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
