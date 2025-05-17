using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Areas.Admin.Services;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using Microsoft.AspNetCore.Authorization;
namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/FeatureSlider")]
    public class FeatureSliderController : Controller
    {
        private readonly IHttpService _httpService;
        public FeatureSliderController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("http://localhost:7070/api/");
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Öne Çıkan Görsel İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Görseller";
            ViewBag.v3 = "Öne Çıkan Görsel Listesi";


            var result = await _httpService.Get<ResultFeatureSliderDto>("FeatureSliders");
            if (result != null) return View(result);
            return View();
        }
            

        [HttpGet]
        [Route("CreateFeatureSlider")]
        public IActionResult CreateFeatureSlider()
        {
            ViewBag.v0 = "Öne Çıkan Görsel İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Görseller";
            ViewBag.v3 = "Öne Çıkan Görsel Ekle";

            return View();
        }

        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto)
        {

            createFeatureSliderDto.Status = false;
            var result = await _httpService.Create<CreateFeatureSliderDto>("FeatureSliders", createFeatureSliderDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteFeatureSlider/{id}")]
        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {

            var result = await _httpService.DeleteById("FeatureSliders", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            ViewBag.v0 = "Öne Çıkan Görsel İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Görseller";
            ViewBag.v3 = "Öne Çıkan Görsel Güncelle";

            var result = await _httpService.GetById<UpdateFeatureSliderDto>("FeatureSliders", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto)
        {

            var result = await _httpService.Update<UpdateFeatureSliderDto>("FeatureSliders", updateFeatureSliderDto);
            if (result) { return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" }); }
            return View();
        }
    }
}
