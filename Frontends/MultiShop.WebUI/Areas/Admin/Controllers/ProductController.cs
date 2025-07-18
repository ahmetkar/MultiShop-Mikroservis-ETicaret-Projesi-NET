using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;

 
        }

        void ProductViewBags(string pagename)
        {
            ViewBag.v0 = "Ürün İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = pagename;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ProductViewBags("Ürün Listesi");

            var values = await _productService.GetAllProductAsync();
            return View(values);
        }


        [Route("ProductListWithCategory")]
        public async Task<IActionResult> ProductListWithCategory()
        {
            ProductViewBags("Ürün Listesi");


           /* var result = await _httpService.Get<ResultProductWithCategory>("Products/ProductListWithCategory");
            if (result != null) return View(result);*/
            return View();
        }

        [HttpGet]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ProductViewBags("Ürün Ekle");

      
            var catresults = await _categoryService.GetAllCategoryAsync();
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
            await _productService.CreateProductAsync(createProductDto);
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }


        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }

        [Route("UpdateProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ProductViewBags("Ürün Güncelle");


            var catresults = await _categoryService.GetAllCategoryAsync();
            if (catresults != null)
            {
                List<SelectListItem> categoryValues = (from c in catresults select new SelectListItem { Text = c.CategoryName, Value = c.CategoryID }).ToList();

                ViewBag.Categories = categoryValues;
            }
            else
            {
                ViewBag.Categories = null;
            }

            var result = await _productService.GetByIdProduct(id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateProduct/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
    }
}
    