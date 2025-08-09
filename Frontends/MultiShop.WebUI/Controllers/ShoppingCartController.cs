using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Identity;

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

            var basketitems = new BasketTotalDto();

            if (User.Identity.IsAuthenticated)
            {
                
             int? isAdded = HttpContext.Session.GetInt32("IsCookiesAdded");
             if (isAdded != 1)
             {
                isAdded = await _basketService.AddCookieDataToDatabase();
             }
               

                basketitems = await _basketService.GetBasketFromDatabase();
            }
            else
            {
                basketitems = await _basketService.GetBasketFromCookies();
            }

                int count = 0;
            count = basketitems.BasketItems.Count;

            if (count > 0)
            {
                ViewBag.TotalPrice = (basketitems).TotalPrice;

                int KDVPercent = 10;
                double KDV = ((double)basketitems.TotalPrice * KDVPercent) / 100;
                ViewBag.KDV = KDV;

                double totalPriceWithTax = (double)basketitems.TotalPrice + KDV;
                ViewBag.TotalPriceWithTax = totalPriceWithTax;
            }


            return View(count);
        }


        public async Task<IActionResult> AddBasketItem(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.AddBasketItemToDatabase(id);
            }
            else
            {
                await _basketService.AddBasketItemToCookies(id);
            }
                
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.RemoveBasketItemFromDatabase(id);
            }
            else
            {
                await _basketService.RemoveBasketItemFromCookies(id);
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecrementBasketItem(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                await _basketService.DecrementBasketItemFromDatabase(id);
            }
            else
            {
                await _basketService.DecrementBasketItemFromCookies(id);
            }
            
            return RedirectToAction("Index");
        }
    }
}
