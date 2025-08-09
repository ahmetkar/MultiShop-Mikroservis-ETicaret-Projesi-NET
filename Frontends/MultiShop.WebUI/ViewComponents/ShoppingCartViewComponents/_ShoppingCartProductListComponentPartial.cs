using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;
using Microsoft.AspNetCore.Identity;
using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.ViewComponents.ShoppingCartViewComponents
{
    public class _ShoppingCartProductListComponentPartial : ViewComponent
    {

        private readonly IBasketService _basketService;

        public _ShoppingCartProductListComponentPartial (IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basketitems = new BasketTotalDto();

            if (User.Identity.IsAuthenticated)
            {
                basketitems = await _basketService.GetBasketFromDatabase();
            }
            else
            {
                basketitems = await _basketService.GetBasketFromCookies();
            }
            return View(basketitems);
        }
    }
}
