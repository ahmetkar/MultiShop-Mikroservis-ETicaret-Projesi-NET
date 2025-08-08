using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public ShoppingCartController
            (IBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(string code,string discountrate,string totalpricewithcoupon)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürünler";
            ViewBag.Directory3 = "Sepetim";


            ViewBag.TotalPriceWithTaxAndCoupon = totalpricewithcoupon;
            ViewBag.code = code;
            ViewBag.DiscountRate = discountrate;


            var basketitems = await _basketService.GetBasket();
            ViewBag.TotalPrice = (basketitems).TotalPrice;

            int KDVPercent = 10;
            double KDV = ((double)basketitems.TotalPrice * KDVPercent) / 100;
            ViewBag.KDV = KDV;

            double totalPriceWithTax = (double)basketitems.TotalPrice + KDV ;
            ViewBag.TotalPriceWithTax = totalPriceWithTax;


           

            return View();
        }


        public async Task<IActionResult> AddBasketItem(string id)
        {
         
            await _basketService.AddBasketItem(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            await _basketService.RemoveBasketItem(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecrementBasketItem(string id)
        {
            await _basketService.DecrementBasketItem(id);
            return RedirectToAction("Index");
        }
    }
}
