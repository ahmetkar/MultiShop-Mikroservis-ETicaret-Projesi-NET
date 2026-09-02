using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.DiscountDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;

        public CouponController(IDiscountService discountService, IProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kuponlar & İndirimler";
            ViewBag.v3 = "Kupon Listesi";
            ViewBag.v0 = "İndirim İşlemleri";

            var coupons = await _discountService.GetAllCouponAsync();
            var products = await _productService.GetAllProductAsync();
            ViewBag.Products = products;
            return View(coupons);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCoupon()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kuponlar & İndirimler";
            ViewBag.v3 = "Yeni Kupon / İndirim Ekle";
            ViewBag.v0 = "İndirim İşlemleri";

            var products = await _productService.GetAllProductAsync();
            var productList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- Tüm Ürünler (Genel Kupon) ---", Value = "" }
            };
            productList.AddRange(products.Select(x => new SelectListItem
            {
                Text = x.ProductName,
                Value = x.ProductId
            }));
            ViewBag.ProductList = productList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CreateDiscountCouponDto createDiscountCouponDto)
        {
            await _discountService.CreateCouponAsync(createDiscountCouponDto);
            return RedirectToAction("Index", "Coupon", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteCoupon(int id)
        {
            await _discountService.DeleteCouponAsync(id);
            return RedirectToAction("Index", "Coupon", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCoupon(int id)
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kuponlar & İndirimler";
            ViewBag.v3 = "Kupon Güncelle";
            ViewBag.v0 = "İndirim İşlemleri";

            var coupon = await _discountService.GetByIdCouponAsync(id);
            var products = await _productService.GetAllProductAsync();
            var productList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- Tüm Ürünler (Genel Kupon) ---", Value = "" }
            };
            productList.AddRange(products.Select(x => new SelectListItem
            {
                Text = x.ProductName,
                Value = x.ProductId,
                Selected = x.ProductId == coupon.ProductId
            }));
            ViewBag.ProductList = productList;

            var updateDto = new UpdateDiscountCouponDto
            {
                CouponId = coupon.CouponId,
                Code = coupon.Code,
                Rate = coupon.Rate,
                IsActive = coupon.IsActive,
                ValidDate = coupon.ValidDate,
                ProductId = coupon.ProductId
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCoupon(UpdateDiscountCouponDto updateDiscountCouponDto)
        {
            await _discountService.UpdateCouponAsync(updateDiscountCouponDto);
            return RedirectToAction("Index", "Coupon", new { area = "Admin" });
        }
    }
}
