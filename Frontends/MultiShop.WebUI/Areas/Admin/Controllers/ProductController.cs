using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Product")]
    public class ProductController : Controller
    {
        private readonly IHttpService _httpService;
        public ProductController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";


            var result = await _httpService.Get<ResultProductDto>("Products");
            if (result != null) return View(result);
            return View();
        }


        [Route("ProductListWithCategory")]
        public async Task<IActionResult> ProductListWithCategory()
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";


            var result = await _httpService.Get<ResultProductWithCategory>("Products/ProductListWithCategory");
            if (result != null) return View(result);
            return View();
        }

        [HttpGet]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Ekle";

            var catresults = await _httpService.Get<ResultCategoryDto>("Categories");
            if(catresults != null)
            {
                List<SelectListItem> categoryValues = (from c in catresults select new SelectListItem {Text = c.CategoryName,Value = c.CategoryID }).ToList();

                ViewBag.Categories = categoryValues;    
            }
            else
            {
                ViewBag.Categories = null;
            }
            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {

            var result = await _httpService.Create<CreateProductDto>("Products", createProductDto);

            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }


        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {

            var result = await _httpService.DeleteById("Products", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Güncelle";

            var catresults = await _httpService.Get<ResultCategoryDto>("Categories");
            if (catresults != null)
            {
                List<SelectListItem> categoryValues = (from c in catresults select new SelectListItem { Text = c.CategoryName, Value = c.CategoryID }).ToList();

                ViewBag.Categories = categoryValues;
            }
            else
            {
                ViewBag.Categories = null;
            }

            var result = await _httpService.GetById<UpdateProductDto>("Products", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateProduct/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {

            var result = await _httpService.Update<UpdateProductDto>("Products", updateProductDto);
            if (result) { return RedirectToAction("Index", "Product", new { area = "Admin" }); }
            return View();
        }
    }
}
    