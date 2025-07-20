using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductDetailServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ProductDetail")]
    public class ProductDetailController : Controller
    {
        

        private readonly IProductDetailService _productDetailService;
        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }


        //id -> product id
        [Route("UpdateProductDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            ViewBag.v0 = "Ürün Detay İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Detayları";
            ViewBag.v3 = "Ürün Detay Güncelle";

            var result = await _productDetailService.GetByProductIdProductDetailForUpdate(id);
        
            if (result != null)
            {
                TempData["MustAdd"] = false;
                return View(result);
            }
            else
            {
                TempData["MustAdd"] = true;
            }
            return View();

        }

        [Route("UpdateProductDetail/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto updateProductDetailDto)
        {
            var result = false;
            if ((bool)TempData.Peek("MustAdd"))
            {
                CreateProductDetailDto createProductDetailDto = new CreateProductDetailDto
                {
                    ProductId = RouteData.Values["id"].ToString(),
                    ProductDescription = updateProductDetailDto.ProductDescription,
                    ProductInfo = updateProductDetailDto.ProductInfo
                };
                await _productDetailService.CreateProductDetailAsync(createProductDetailDto);
            }
            else
            {
                await _productDetailService.UpdateProductDetailAsync(updateProductDetailDto);
            }
            return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
        }
    }
}

