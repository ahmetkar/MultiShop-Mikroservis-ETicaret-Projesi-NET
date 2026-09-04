using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.DiscountServices;
using Microsoft.AspNetCore.Identity;
using MultiShop.DtoLayer.BasketDtos;

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
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["CouponError"] = "Lütfen bir indirim kupon kodu giriniz.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var cleanCode = code.Trim();
            var value = await _discountService.GetDiscountCouponCountRate(cleanCode);

            if (value <= 0)
            {
                TempData["CouponError"] = "Girdiğiniz kupon kodu geçersiz veya süresi dolmuş.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var basketitems = new BasketTotalDto();

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                basketitems = await _basketService.GetBasketFromDatabase();
                if (basketitems != null)
                {
                    await _basketService.SaveBasketToDatabase(basketitems, cleanCode, value);
                }
            }
            else
            {
                basketitems = await _basketService.GetBasketFromCookies();
                if (basketitems != null)
                {
                    await _basketService.SaveBasketToCookies(basketitems, cleanCode, value);
                }
            }

            TempData["CouponSuccess"] = $"'{cleanCode}' kupon kodu başarıyla uygulandı! (%{value} indirim)";
            return RedirectToAction("Index", "ShoppingCart");
        }


    }
}
