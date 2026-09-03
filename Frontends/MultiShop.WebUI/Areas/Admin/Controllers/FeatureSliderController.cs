using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/FeatureSlider")]
    public class FeatureSliderController : Controller
    {
        private readonly IFeatureSliderService _featureSliderService;
        private readonly IProductService _productService;

        public FeatureSliderController(IFeatureSliderService featureSliderService, IProductService productService)
        {
            _featureSliderService = featureSliderService;
            _productService = productService;
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
        public async Task<IActionResult> CreateFeatureSlider()
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Ekle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto, List<string>? selectedProductIds)
        {
            createFeatureSliderDto.Status = true;
            createFeatureSliderDto.ProductIds = selectedProductIds ?? new List<string>();
            await _featureSliderService.CreateFeatureSliderAsync(createFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("DeleteFeatureSlider/{id}")]
        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {
            await _featureSliderService.DeleteFeatureSliderAsync(id);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            FeatureSliderViewBag("Öne Çıkan Görsel Güncelle");
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;

            var result = await _featureSliderService.GetByIdFeatureSlider(id);
            var updateDto = new UpdateFeatureSliderDto
            {
                FeatureSliderID = result.FeatureSliderID,
                Title = result.Title,
                Description = result.Description,
                ImageUrl = result.ImageUrl,
                Status = result.Status,
                ProductIds = result.ProductIds ?? new List<string>()
            };
            return View(updateDto);
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto, List<string>? selectedProductIds)
        {
            updateFeatureSliderDto.ProductIds = selectedProductIds ?? new List<string>();
            await _featureSliderService.UpdateFeatureSliderAsync(updateFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }
    }
}
