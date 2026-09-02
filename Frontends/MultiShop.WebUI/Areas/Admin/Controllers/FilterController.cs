using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FilterDtos;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FilterController : Controller
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Filtreler";
            ViewBag.v3 = "Filtre Listesi";
            ViewBag.v0 = "Filtre İşlemleri";
            var values = await _filterService.GetAllFilterAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateFilter()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Filtreler";
            ViewBag.v3 = "Yeni Filtre Ekle";
            ViewBag.v0 = "Filtre İşlemleri";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilter(CreateFilterDto createFilterDto)
        {
            await _filterService.CreateFilterAsync(createFilterDto);
            return RedirectToAction("Index", "Filter", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteFilter(string id)
        {
            await _filterService.DeleteFilterAsync(id);
            return RedirectToAction("Index", "Filter", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFilter(string id)
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Filtreler";
            ViewBag.v3 = "Filtre Güncelle";
            ViewBag.v0 = "Filtre İşlemleri";
            var value = await _filterService.GetByIdFilterAsync(id);
            var updateDto = new UpdateFilterDto
            {
                FilterId = value.FilterId,
                FilterTitle = value.FilterTitle,
                FilterName = value.FilterName
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFilter(UpdateFilterDto updateFilterDto)
        {
            await _filterService.UpdateFilterAsync(updateFilterDto);
            return RedirectToAction("Index", "Filter", new { area = "Admin" });
        }
    }
}
