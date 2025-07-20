using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IOfferDiscountService _offerDiscountService;
        public OfferDiscountController(IOfferDiscountService offerDiscountService)
        {
            _offerDiscountService = offerDiscountService;
        }

        void ViewBagList(string pagename)
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifiler";
            ViewBag.v3 = pagename;
        }


        public async Task<IActionResult> Index()
        {

            ViewBagList("Özel İndirim Teklifi Listesi");

            var result = await _offerDiscountService.GetAllOfferDiscountAsync();
            return View(result);
        }


        [HttpGet]
        [Route("CreateOfferDiscount")]
        public IActionResult CreateOfferDiscount()
        {
            ViewBagList("Özel İndirim Teklifi Ekle");

            return View();
        }

        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _offerDiscountService.CreateOfferDiscountAsync(createOfferDiscountDto);

            return RedirectToAction("Index","OfferDiscount", new { area = "Admin" });
        }


        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {

            await _offerDiscountService.DeleteOfferDiscountAsync(id);
          
            return RedirectToAction("Index","OfferDiscount", new { area = "Admin" });
           
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBagList("Özel İndirim Teklifi Güncelle");

            var result = await _offerDiscountService.GetByIdOfferDiscount(id);
            return View(result);
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            await _offerDiscountService.UpdateOfferDiscountAsync(updateOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" }); 
        }
    }
}
