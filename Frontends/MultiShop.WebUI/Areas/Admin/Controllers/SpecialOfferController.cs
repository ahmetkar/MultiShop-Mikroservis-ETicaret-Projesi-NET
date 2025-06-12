using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDTOs;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/SpecialOffer")]
    public class SpecialOfferController : Controller
    {
        private readonly IHttpService _httpService;
        public SpecialOfferController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Özel Teklif İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Listesi";


            var result = await _httpService.Get<ResultSpecialOfferDto>("SpecialOffers");
            if (result != null) return View(result);
            return View();
        }


        [HttpGet]
        [Route("CreateSpecialOffer")]
        public IActionResult CreateSpecialOffer()
        {
            ViewBag.v0 = "Özel Teklif İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Ekle";

            return View();
        }

        [HttpPost]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto)
        {


            var result = await _httpService.Create<CreateSpecialOfferDto>("SpecialOffers", createSpecialOfferDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteSpecialOffer/{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {


            var result = await _httpService.DeleteById("SpecialOffers", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            ViewBag.v0 = "Özel Teklif İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Güncelle";

            var result = await _httpService.GetById<UpdateSpecialOfferDto>("SpecialOffers", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto)
        {

            var result = await _httpService.Update<UpdateSpecialOfferDto>("SpecialOffers", updateSpecialOfferDto);
            if (result) { return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" }); }
            return View();
        }
    }
}
