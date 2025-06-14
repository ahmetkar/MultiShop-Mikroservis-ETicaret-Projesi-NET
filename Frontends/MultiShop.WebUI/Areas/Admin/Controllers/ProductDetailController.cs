using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDTOs;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductDetail")]
    public class ProductDetailController : Controller
    {
        
        private readonly IHttpService _httpService;
        public ProductDetailController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }


        [Route("UpdateProductDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            ViewBag.v0 = "Ürün Detay İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürün Detayları";
            ViewBag.v3 = "Ürün Detay Güncelle";

            var result = await _httpService.GetOne<UpdateProductDetailDto>("ProductDetails/GetProductDetailByProductId?id=" + id);
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
                result = await _httpService.Create<CreateProductDetailDto>("ProductDetails", createProductDetailDto);
            }
            else
            {
                result = await _httpService.Update<UpdateProductDetailDto>("ProductDetails", updateProductDetailDto);
            }
            if (result) { return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" }); }
            return View();
        }
    }
}

