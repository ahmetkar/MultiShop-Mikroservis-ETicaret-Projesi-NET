using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Attributes;
using MultiShop.WebUI.Services.BasketServices;

namespace MultiShop.WebUI.Controllers
{
    [OrderAuthorize]
    public class PaymentController : Controller
    {
       
        private readonly IBasketService _basketService;

        public PaymentController(IBasketService basketService)
        {
         
            _basketService = basketService;
        }
        public async Task<IActionResult> Index()
        {
           
            int count = 0;
            var basket = await _basketService.GetBasketFromDatabase();


            count = basket.BasketItems.Count;

            
            if (count == 0) return RedirectToAction("Index","Default");
            return View();
        }
    }
}
