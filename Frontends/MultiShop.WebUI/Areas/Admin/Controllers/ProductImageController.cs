using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductImageServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ProductImage")]
    public class ProductImageController : Controller
    {

        private readonly IProductImageService _productImageService;
        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
    
        [Route("UpdateProductImage/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductImage(string id)
        {
            ViewBag.v0 = "Ürün Resimi İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Resimiler";
            ViewBag.v3 = "Ürün Resimi Güncelle";

            var result = await _productImageService.GetByProductIdProductImagesAsync(id);
            if (result != null)
            {
                TempData["MustAdd"] = false;    
                return View(result);
            }else
            {
                TempData["MustAdd"] = true;
            }
            return View();
            
        }

        [Route("UpdateProductImage/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
        {
            var result = false;
            if ((bool)TempData.Peek("MustAdd"))
            {
                CreateProductImageDto createProductImageDto = new CreateProductImageDto
                {
                    ProductId = RouteData.Values["id"].ToString() ,
                    Image1 = updateProductImageDto.Image1,
                    Image2 = updateProductImageDto.Image2,
                    Image3 = updateProductImageDto.Image3,
                    Image4 = updateProductImageDto.Image4
                };
               await _productImageService.CreateProductImageAsync(createProductImageDto);
               
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            }
            return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
        }
    }
}
