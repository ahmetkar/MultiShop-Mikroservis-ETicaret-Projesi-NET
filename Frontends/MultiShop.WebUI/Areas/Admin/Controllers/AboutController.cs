using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/About")]
    public class AboutController : Controller
    {
        private readonly IHttpService _httpService;
        public AboutController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Hakkımızda Alanı İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Hakkımızda Alanı";
            ViewBag.v3 = "Hakkımızda Alanı Listesi";


            var result = await _httpService.Get<ResultAboutDto>("Abouts/AboutList");
            if (result != null) return View(result);
            return View();
        }


        [HttpGet]
        [Route("CreateAbout")]
        public IActionResult CreateAbout()
        {
            ViewBag.v0 = "Hakkımızda Alanı İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Hakkımızda Alanları";
            ViewBag.v3 = "Hakkımızda Alanı Ekle";

            return View();
        }

        [HttpPost]
        [Route("CreateAbout")]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {


            var result = await _httpService.Create<CreateAboutDto>("Abouts", createAboutDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteAbout/{id}")]
        public async Task<IActionResult> DeleteAbout(string id)
        {


            var result = await _httpService.DeleteById("Abouts", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateAbout/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateAbout(string id)
        {
            ViewBag.v0 = "Hakkımızda Alanı İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Hakkımızda Alanları";
            ViewBag.v3 = "Hakkımızda Alanı Güncelle";

            var result = await _httpService.GetById<UpdateAboutDto>("Abouts", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateAbout/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {


            var result = await _httpService.Update<UpdateAboutDto>("Abouts", updateAboutDto);
            if (result) { return RedirectToAction("Index", "About", new { area = "Admin" }); }
            return View();
        }
    }
}
