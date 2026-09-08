using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.FileUploadServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFilterService _filterService;
        private readonly IFileUploadService _fileUploadService;

        public CategoryController(
            ICategoryService categoryService,
            IFilterService filterService,
            IFileUploadService fileUploadService)
        {
            _categoryService = categoryService;
            _filterService = filterService;
            _fileUploadService = fileUploadService;
        }

        void CategoryViewBags(string pagename)
        {
            ViewBag.v0 = "Kategori İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = pagename;
        }

        public async Task<IActionResult> Index()
        {
            CategoryViewBags("Kategori Listesi");
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }

        [HttpGet]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory()
        {
            CategoryViewBags("Kategori Ekle");
            var filters = await _filterService.GetAllFilterAsync();
            ViewBag.AllFilters = filters;
            await LoadExistingImagesToViewBag();
            return View();
        }

        private async Task LoadExistingImagesToViewBag()
        {
            var existingImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var categories = await _categoryService.GetAllCategoryAsync();
                if (categories != null)
                {
                    foreach (var c in categories)
                    {
                        if (!string.IsNullOrWhiteSpace(c.ImageUrl))
                            existingImages.Add(c.ImageUrl.Trim());
                    }
                }
            }
            catch { }

            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "categories");
                if (Directory.Exists(webRoot))
                {
                    var files = Directory.GetFiles(webRoot);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        existingImages.Add($"/images/categories/{fileName}");
                    }
                }
            }
            catch { }

            ViewBag.ExistingImages = existingImages.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(
            CreateCategoryDto createCategoryDto,
            IFormFile? imageFile,
            List<string>? selectedFilterIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "categories");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    createCategoryDto.ImageUrl = uploaded;
                }
            }

            createCategoryDto.SelectedFilterIds = selectedFilterIds ?? new List<string>();
            await _categoryService.CreateCatagoryAsync(createCategoryDto);
            return RedirectToAction("Index","Category", new { area = "Admin" });
        }

        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }

        [Route("UpdateCategory/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            CategoryViewBags("Kategori Güncelle");
            var value = await _categoryService.GetByIdCategory(id);
            var filters = await _filterService.GetAllFilterAsync();
            ViewBag.AllFilters = filters;

            var realvalue = new UpdateCategoryDto
            {
                CategoryID = value.CategoryID,
                CategoryName = value.CategoryName,
                ImageUrl = value.ImageUrl,
                SelectedFilterIds = value.SelectedFilterIds ?? new List<string>()
            };
            await LoadExistingImagesToViewBag();
            return View(realvalue);
        }

        [Route("UpdateCategory/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(
            UpdateCategoryDto updateCategoryDto,
            IFormFile? imageFile,
            List<string>? selectedFilterIds)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploaded = await _fileUploadService.UploadFileAsync(imageFile, "categories");
                if (!string.IsNullOrEmpty(uploaded))
                {
                    updateCategoryDto.ImageUrl = uploaded;
                }
            }

            updateCategoryDto.SelectedFilterIds = selectedFilterIds ?? new List<string>();
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index","Category", new { area = "Admin" }); 
        }
    }
}
