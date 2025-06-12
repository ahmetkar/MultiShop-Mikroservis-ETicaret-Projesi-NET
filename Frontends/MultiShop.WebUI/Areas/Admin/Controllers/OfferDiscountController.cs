using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IHttpService _httpService;
        public OfferDiscountController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifiler";
            ViewBag.v3 = "Özel İndirim Teklifi Listesi";


            var result = await _httpService.Get<ResultOfferDiscountDto>("OfferDiscounts");
            if (result != null) return View(result);
            return View();
        }


        [HttpGet]
        [Route("CreateOfferDiscount")]
        public IActionResult CreateOfferDiscount()
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifiler";
            ViewBag.v3 = "Özel İndirim Teklifi Ekle";

            return View();
        }

        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto createOfferDiscountDto)
        {


            var result = await _httpService.Create<CreateOfferDiscountDto>("OfferDiscounts", createOfferDiscountDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {


            var result = await _httpService.DeleteById("OfferDiscounts", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifiler";
            ViewBag.v3 = "Özel İndirim Teklifi Güncelle";

            var result = await _httpService.GetById<UpdateOfferDiscountDto>("OfferDiscounts", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto)
        {


            var result = await _httpService.Update<UpdateOfferDiscountDto>("OfferDiscounts", updateOfferDiscountDto);
            if (result) { return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" }); }
            return View();
        }
    }
}
