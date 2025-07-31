using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IBasketService _basketService;

        public DiscountController(IDiscountService discountService,IBasketService basketService)
        {
            _discountService = discountService;
            _basketService = basketService;
        }


        [HttpGet]
        public async Task<PartialViewResult> ConfirmDiscountCoupon()
        {
            return PartialView();
        }



        [HttpPost]
        public async Task<IActionResult> ConfirmDiscountCoupon(string code)
        {
        
            var value = await _discountService.GetDiscountCouponCountRate(code);

            int KDVPercent = 10;
            var basketitems = await _basketService.GetBasket();
            double totalPriceWithTax = (double)basketitems.TotalPrice + ((double)basketitems.TotalPrice * KDVPercent) / 100;
            double totalprice = totalPriceWithTax - (totalPriceWithTax * value) / 100;


            return RedirectToAction("Index","ShoppingCart",new {code = code,discountrate = value,totalpricewithcoupon = totalprice});
        }


    }
}
