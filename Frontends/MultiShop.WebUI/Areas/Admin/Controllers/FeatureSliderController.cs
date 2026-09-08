using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.FileUploadServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/FeatureSlider")]
    public class FeatureSliderController : Controller
    {
        private readonly IFeatureSliderService _featureSliderService;
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;

        public FeatureSliderController(
            IFeatureSliderService featureSliderService,
            IProductService productService,
            IFileUploadService fileUploadService)
        {
            _featureSliderService = featureSliderService;
            _productService = productService;
            _fileUploadService = fileUploadService;
        }

        void FeatureSliderViewBag(string pagename)
        {
            ViewBag.v0 = "Öne Çıkan Görsel İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Görseller";
            ViewBag.v3 = pagename;
        }

        public async Task<IActionResult> Index()
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Listesi");
            var result = await _featureSliderService.GetAllFeatureSliderAsync();
            return View(result);
        }

        [HttpGet]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider()
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Ekle");
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
                var sliders = await _featureSliderService.GetAllFeatureSliderAsync();
                if (sliders != null)
                {
                    foreach (var s in sliders)
                    {
                        if (!string.IsNullOrWhiteSpace(s.ImageUrl))
                            existingImages.Add(s.ImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sliders");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/sliders/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(
            CreateFeatureSliderDto createFeatureSliderDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "sliders");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    createFeatureSliderDto.ImageUrl = uploaded;
                }
            }

            createFeatureSliderDto.Status = true;
            createFeatureSliderDto.ProductIds = selectedProductIds ?? new List<string>();
            await _featureSliderService.CreateFeatureSliderAsync(createFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("DeleteFeatureSlider/{id}")]
        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {
            await _featureSliderService.DeleteFeatureSliderAsync(id);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _featureSliderService.GetByIdFeatureSlider(id);
            var updateDto = new UpdateFeatureSliderDto
            {
                FeatureSliderID = result.FeatureSliderID,
                Title = result.Title,
                Description = result.Description,
                ImageUrl = result.ImageUrl,
                Status = result.Status,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            await LoadExistingImagesToViewBag();
            return View(updateDto);
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(
            UpdateFeatureSliderDto updateFeatureSliderDto,
            IFormFile? imageFile,
            List<string>? selectedProductIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "sliders");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    updateFeatureSliderDto.ImageUrl = uploaded;
                }
            }

            updateFeatureSliderDto.ProductIds = selectedProductIds ?? new List<string>();
            await _featureSliderService.UpdateFeatureSliderAsync(updateFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }
    }
}
