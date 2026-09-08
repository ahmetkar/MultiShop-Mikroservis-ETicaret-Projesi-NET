using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.FileUploadServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IOfferDiscountService _offerDiscountService;
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;

        public OfferDiscountController(
            IOfferDiscountService offerDiscountService,
            IProductService productService,
            IFileUploadService fileUploadService)
        {
            _offerDiscountService = offerDiscountService;
            _productService = productService;
            _fileUploadService = fileUploadService;
        }

        void ViewBagList(string pagename)
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifileri";
            ViewBag.v3 = pagename;
        }

        public async Task<IActionResult> Index()
        {
            ViewBagList("Özel İndirim Teklifi Listesi");
            var result = await _offerDiscountService.GetAllOfferDiscountAsync();
            return View(result);
        }

        [HttpGet]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount()
        {
            ViewBagList("Özel İndirim Teklifi Ekle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;
            await LoadExistingImagesToViewBag();
            return View();
        }

        private async Task LoadExistingImagesToViewBag()
        {
            var existingImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var discounts = await _offerDiscountService.GetAllOfferDiscountAsync();
                if (discounts != null)
                {
                    foreach (var d in discounts)
                    {
                        if (!string.IsNullOrWhiteSpace(d.ImageUrl))
                            existingImages.Add(d.ImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "offers");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/offers/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(
            CreateOfferDiscountDto createOfferDiscountDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "offers");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    createOfferDiscountDto.ImageUrl = uploaded;
                }
            }

            createOfferDiscountDto.ProductIds = selectedProductIds ?? new List<string>();
            await _offerDiscountService.CreateOfferDiscountAsync(createOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }

        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {
            await _offerDiscountService.DeleteOfferDiscountAsync(id);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBagList("Özel İndirim Teklifi Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _offerDiscountService.GetByIdOfferDiscount(id);
            var updateDto = new UpdateOfferDiscountDto
            {
                OfferDiscountId = result.OfferDiscountId,
                Title = result.Title,
                Subtitle = result.Subtitle,
                ImageUrl = result.ImageUrl,
                ButtonTitle = result.ButtonTitle,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            await LoadExistingImagesToViewBag();
            return View(updateDto);
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfferDiscount(
            UpdateOfferDiscountDto updateOfferDiscountDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "offers");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    updateOfferDiscountDto.ImageUrl = uploaded;
                }
            }

            updateOfferDiscountDto.ProductIds = selectedProductIds ?? new List<string>();
            await _offerDiscountService.UpdateOfferDiscountAsync(updateOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" }); 
        }
    }
}
