using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductImageServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ProductImage")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;
        private readonly IProductService _productService;

        public ProductImageController(IProductImageService productImageService, IProductService productService)
        {
            _productImageService = productImageService;
            _productService = productService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Görsel / Resim Yönetimi";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Resimleri";
            ViewBag.v3 = "Görsel Listesi";

            var images = await _productImageService.GetAllProductImageAsync();
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products ?? new List<ResultProductDto>();

            return View(images ?? new List<ResultProductImageDto>());
        }

        [HttpGet("CreateProductImage")]
        public async Task<IActionResult> CreateProductImage(string? productId)
        {
            ViewBag.v0 = "Görsel Ekleme";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Resimleri";
            ViewBag.v3 = "Yeni Görsel Ekle";

            var products = await _productService.GetAllProductAsync();
            ViewBag.ProductList = (from x in products
                                   select new SelectListItem
                                   {
                                       Text = x.ProductName,
                                       Value = x.ProductId,
                                       Selected = (x.ProductId == productId)
                                   }).ToList();

            var model = new CreateProductImageDto();
            if (!string.IsNullOrEmpty(productId))
            {
                model.ProductId = productId;
            }

            return View(model);
        }

        [HttpPost("CreateProductImage")]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto createProductImageDto)
        {
            if (string.IsNullOrWhiteSpace(createProductImageDto.ProductId))
            {
                ModelState.AddModelError("", "Lütfen bir ürün seçiniz.");
                var products = await _productService.GetAllProductAsync();
                ViewBag.ProductList = (from x in products
                                       select new SelectListItem
                                       {
                                           Text = x.ProductName,
                                           Value = x.ProductId
                                       }).ToList();
                return View(createProductImageDto);
            }

            await _productImageService.CreateProductImageAsync(createProductImageDto);
            return RedirectToAction("Index", "ProductImage", new { area = "Admin" });
        }

        [HttpGet("UpdateProductImage/{id}")]
        public async Task<IActionResult> UpdateProductImage(string id)
        {
            ViewBag.v0 = "Görsel Güncelleme";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Resimleri";
            ViewBag.v3 = "Görselleri Güncelle";

            GetByIdProductImageDto? result = null;
            try
            {
                result = await _productImageService.GetByIdProductImage(id);
            }
            catch { }

            if (result == null || string.IsNullOrEmpty(result.ProductImageID))
            {
                try
                {
                    result = await _productImageService.GetByProductIdProductImagesAsync(id);
                }
                catch { }
            }

            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products ?? new List<ResultProductDto>();

            if (result != null && !string.IsNullOrEmpty(result.ProductImageID))
            {
                ViewBag.IsNew = false;
                var updateDto = new UpdateProductImageDto
                {
                    ProductImageID = result.ProductImageID,
                    ProductId = result.ProductId,
                    Image1 = result.Image1,
                    Image2 = result.Image2,
                    Image3 = result.Image3,
                    Image4 = result.Image4
                };
                return View(updateDto);
            }
            else
            {
                ViewBag.IsNew = true;
                var newDto = new UpdateProductImageDto
                {
                    ProductId = id
                };
                return View(newDto);
            }
        }

        [HttpPost("UpdateProductImage/{id}")]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
        {
            if (string.IsNullOrEmpty(updateProductImageDto.ProductImageID))
            {
                var createDto = new CreateProductImageDto
                {
                    ProductId = updateProductImageDto.ProductId ?? RouteData.Values["id"]?.ToString() ?? "",
                    Image1 = updateProductImageDto.Image1,
                    Image2 = updateProductImageDto.Image2,
                    Image3 = updateProductImageDto.Image3,
                    Image4 = updateProductImageDto.Image4
                };
                await _productImageService.CreateProductImageAsync(createDto);
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            }

            return RedirectToAction("Index", "ProductImage", new { area = "Admin" });
        }

        [HttpGet("DeleteProductImage/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _productImageService.DeleteProductImageAsync(id);
            return RedirectToAction("Index", "ProductImage", new { area = "Admin" });
        }
    }
}

