using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Areas.Admin.Services;
using Newtonsoft.Json;
using System.Threading.Tasks.Dataflow;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {

        private readonly IHttpService _httpService;
        public CategoryController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("http://localhost:7070/api/");
        }

      
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Kategori İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Listesi";

      
            var result = await _httpService.Get<ResultCategoryDto>("Categories");
            if (result != null) return View(result);
            return View();
        }


        [HttpGet]
        [Route("CreateCategory")]
        public IActionResult CreateCategory()
        {
            ViewBag.v0 = "Kategori İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Ekle";

            return View();
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
       

            var result = await _httpService.Create<CreateCategoryDto>("Categories", createCategoryDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
         

            var result = await _httpService.DeleteById("Categories", id);
            if(result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateCategory/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            ViewBag.v0 = "Kategori İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Güncelle";

            var result = await _httpService.GetById<UpdateCategoryDto>("Categories", id);
            if(result != null) return View(result);
            return View();
        }

        [Route("UpdateCategory/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
      

            var result = await _httpService.Update<UpdateCategoryDto>("Categories", updateCategoryDto);
            if (result) { return RedirectToAction("Index","Category", new { area = "Admin" }); }
            return View();
        }
    }
}
