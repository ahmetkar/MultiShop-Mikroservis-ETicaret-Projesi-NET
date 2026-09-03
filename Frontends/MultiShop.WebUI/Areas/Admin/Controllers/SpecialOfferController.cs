using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/SpecialOffer")]
    public class SpecialOfferController : Controller
    {
        private readonly ISpecialOfferService _specialOfferService;
        private readonly IProductService _productService;

        public SpecialOfferController(ISpecialOfferService specialOfferService, IProductService productService)
        {
            _specialOfferService = specialOfferService;
            _productService = productService;
        }

        void SpecialOfferViewBag(string pagename)
        {
            ViewBag.v0 = "Özel Teklif İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = pagename;
        }
         
        public async Task<IActionResult> Index()
        {
            SpecialOfferViewBag("Özel Teklif Listesi");
            var result = await _specialOfferService.GetAllSpecialOfferAsync();
            return View(result);
        }

        [HttpGet]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer()
        {
            SpecialOfferViewBag("Özel Teklif Ekle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto, List<string>? selectedProductIds)
        {
            createSpecialOfferDto.ProductIds = selectedProductIds ?? new List<string>();
            await _specialOfferService.CreateSpecialOfferAsync(createSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }

        [Route("DeleteSpecialOffer/{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            await _specialOfferService.DeleteSpecialOfferAsync(id);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            SpecialOfferViewBag("Özel Teklif Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _specialOfferService.GetByIdSpecialOffer(id);
            var updateDto = new UpdateSpecialOfferDto
            {
                SpecialOfferId = result.SpecialOfferId,
                Title = result.Title,
                Subtitle = result.Subtitle,
                ImageUrl = result.ImageUrl,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            return View(updateDto);
        }

        [Route("UpdateSpecialOffer/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto, List<string>? selectedProductIds)
        {
            updateSpecialOfferDto.ProductIds = selectedProductIds ?? new List<string>();
            await _specialOfferService.UpdateSpecialOfferAsync(updateSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }
    }
}
