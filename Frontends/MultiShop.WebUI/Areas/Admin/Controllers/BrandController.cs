using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.BrandDtos;
using MultiShop.WebUI.Services.CatalogServices.BrandServices;
using MultiShop.WebUI.Services.FileUploadServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Brand")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IFileUploadService _fileUploadService;

        public BrandController(IBrandService brandService, IFileUploadService fileUploadService)
        {
            _brandService = brandService;
            _fileUploadService = fileUploadService;
        }

        void ViewBagList(string pagename)
        {
            ViewBag.v0 = "Marka İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Markalar";
            ViewBag.v3 = pagename;
        }

        public async Task<IActionResult> Index()
        {
            ViewBagList("Marka Listesi");
            var result = await _brandService.GetAllBrandAsync();
            return View(result);
        }

        [HttpGet]
        [Route("CreateBrand")]
        public async Task<IActionResult> CreateBrand()
        {
            ViewBagList("Marka Ekle");
            await LoadExistingImagesToViewBag();
            return View();
        }

        private async Task LoadExistingImagesToViewBag()
        {
            var existingImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var brands = await _brandService.GetAllBrandAsync();
                if (brands != null)
                {
                    foreach (var b in brands)
                    {
                        if (!string.IsNullOrWhiteSpace(b.ImageUrl))
                            existingImages.Add(b.ImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "brands");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/brands/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        [HttpPost]
        [Route("CreateBrand")]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "brands");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    createBrandDto.ImageUrl = uploaded;
                }
            }

            await _brandService.CreateBrandAsync(createBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

        [Route("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

        [Route("UpdateBrand/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            ViewBagList("Marka Güncelle");
            var result = await _brandService.GetByIdBrand(id);
            await LoadExistingImagesToViewBag();
            return View(result);
        }

        [Route("UpdateBrand/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "brands");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    updateBrandDto.ImageUrl = uploaded;
                }
            }

            await _brandService.UpdateBrandAsync(updateBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }
    }
}

