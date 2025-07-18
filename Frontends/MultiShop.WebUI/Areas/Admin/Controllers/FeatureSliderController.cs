using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using Microsoft.AspNetCore.Authorization;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.CatalogServices.SliderServices;
namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/FeatureSlider")]
    public class FeatureSliderController : Controller
    {
        private readonly IFeatureSliderService _featureSliderService;
        public FeatureSliderController(IHttpService httpService,IFeatureSliderService featureSliderService)
        {
            _featureSliderService = featureSliderService;

        
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
        public IActionResult CreateFeatureSlider()
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Ekle");

            return View();
        }

        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto)
        {

            createFeatureSliderDto.Status = false;
            await _featureSliderService.CreateFeatureSliderAsync(createFeatureSliderDto);

            return RedirectToAction("Index","FeatureSlider", new { area = "Admin" });
        }


        [Route("DeleteFeatureSlider/{id}")]
        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {

            await _featureSliderService.DeleteFeatureSliderAsync(id);
            return RedirectToAction("Index", new { area = "Admin" }); 
       
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Güncelle");

            var result = await _featureSliderService.GetByIdFeatureSlider(id);
            return View(result);
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            await _featureSliderService.UpdateFeatureSliderAsync(updateFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" }); 
        }
    }
}
