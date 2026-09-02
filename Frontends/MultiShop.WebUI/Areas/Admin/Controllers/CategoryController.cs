using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFilterService _filterService;

        public CategoryController(ICategoryService categoryService, IFilterService filterService)
        {
            _categoryService = categoryService;
            _filterService = filterService;
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
            return View();
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto, List<string>? selectedFilterIds)
        {
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
            return View(realvalue);
        }

        [Route("UpdateCategory/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto, List<string>? selectedFilterIds)
        {
            updateCategoryDto.SelectedFilterIds = selectedFilterIds ?? new List<string>();
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index","Category", new { area = "Admin" }); 
        }
    }
}
