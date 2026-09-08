using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;
using MultiShop.WebUI.Services.FileUploadServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/SpecialOffer")]
    public class SpecialOfferController : Controller
    {
        private readonly ISpecialOfferService _specialOfferService;
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;

        public SpecialOfferController(
            ISpecialOfferService specialOfferService,
            IProductService productService,
            IFileUploadService fileUploadService)
        {
            _specialOfferService = specialOfferService;
            _productService = productService;
            _fileUploadService = fileUploadService;
        }

        void SpecialOfferViewBag(string pagename)
        {
            ViewBag.v0 = "Özel Teklif İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = pagename;
        }
         
        public async Task<IActionResult> Index()
        {
            SpecialOfferViewBag("Özel Teklif Listesi");
            var result = await _specialOfferService.GetAllSpecialOfferAsync();
            return View(result);
        }

        [HttpGet]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer()
        {
            SpecialOfferViewBag("Özel Teklif Ekle");
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
                var offers = await _specialOfferService.GetAllSpecialOfferAsync();
                if (offers != null)
                {
                    foreach (var o in offers)
                    {
                        if (!string.IsNullOrWhiteSpace(o.ImageUrl))
                            existingImages.Add(o.ImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "specialoffers");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/specialoffers/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        [HttpPost]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer(
            CreateSpecialOfferDto createSpecialOfferDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "specialoffers");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    createSpecialOfferDto.ImageUrl = uploaded;
                }
            }

            createSpecialOfferDto.ProductIds = selectedProductIds ?? new List<string>();
            await _specialOfferService.CreateSpecialOfferAsync(createSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }

        [Route("DeleteSpecialOffer/{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            await _specialOfferService.DeleteSpecialOfferAsync(id);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            SpecialOfferViewBag("Özel Teklif Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _specialOfferService.GetByIdSpecialOffer(id);
            var updateDto = new UpdateSpecialOfferDto
            {
                SpecialOfferId = result.SpecialOfferId,
                Title = result.Title,
                Subtitle = result.Subtitle,
                ImageUrl = result.ImageUrl,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            await LoadExistingImagesToViewBag();
            return View(updateDto);
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(
            UpdateSpecialOfferDto updateSpecialOfferDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "specialoffers");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    updateSpecialOfferDto.ImageUrl = uploaded;
                }
            }

            updateSpecialOfferDto.ProductIds = selectedProductIds ?? new List<string>();
            await _specialOfferService.UpdateSpecialOfferAsync(updateSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }
    }
}
