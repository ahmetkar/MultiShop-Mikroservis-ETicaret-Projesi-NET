using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IOfferDiscountService _offerDiscountService;
        private readonly IProductService _productService;

        public OfferDiscountController(IOfferDiscountService offerDiscountService, IProductService productService)
        {
            _offerDiscountService = offerDiscountService;
            _productService = productService;
        }

        void ViewBagList(string pagename)
        {
            ViewBag.v0 = "Özel İndirim Teklifi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel İndirim Teklifileri";
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
        public async Task<IActionResult> CreateOfferDiscount()
        {
            ViewBagList("Özel İndirim Teklifi Ekle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto createOfferDiscountDto, List<string>? selectedProductIds)
        {
            createOfferDiscountDto.ProductIds = selectedProductIds ?? new List<string>();
            await _offerDiscountService.CreateOfferDiscountAsync(createOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }

        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {
            await _offerDiscountService.DeleteOfferDiscountAsync(id);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBagList("Özel İndirim Teklifi Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _offerDiscountService.GetByIdOfferDiscount(id);
            var updateDto = new UpdateOfferDiscountDto
            {
                OfferDiscountId = result.OfferDiscountId,
                Title = result.Title,
                Subtitle = result.Subtitle,
                ImageUrl = result.ImageUrl,
                ButtonTitle = result.ButtonTitle,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            return View(updateDto);
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto, List<string>? selectedProductIds)
        {
            updateOfferDiscountDto.ProductIds = selectedProductIds ?? new List<string>();
            await _offerDiscountService.UpdateOfferDiscountAsync(updateOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" }); 
        }
    }
}
