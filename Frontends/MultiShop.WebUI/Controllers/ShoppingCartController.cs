using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly IUserService _userService;
        private readonly IDataProtector _protector;

        public ShoppingCartController
            (IBasketService basketService, IProductService productService, IOrderOderingService orderOderingService, IUserService userService,
            IDataProtectionProvider provider)
        {
            _basketService = basketService;
            _productService = productService;
            _orderOderingService = orderOderingService;
            _userService = userService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");

        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürünler";
            ViewBag.Directory3 = "Sepetim";



          

            var basketitems = new BasketTotalDto();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("sub")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("nameidentifier")?.Value;

                var activeOrdering = await _orderOderingService.GetActiveOrderingByUserId(userId);

                if (activeOrdering != null)
                {
                    if (activeOrdering.Status != OrderStatus.PaymentCompleted && activeOrdering.Status != OrderStatus.CargoFailed &&
                        activeOrdering.Status != OrderStatus.CargoCreated && activeOrdering.Status != OrderStatus.OrderNotCreated
                        )
                    {
                       
                        var encryptedId = _protector.Protect(activeOrdering.OrderingId.ToString());
                        return RedirectToAction("Index", "Payment", new { ActiveOrderingId = encryptedId });
                    }
                }
                

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
                ViewBag.TotalPriceWithoutKDV = basketitems.TotalPriceWithoutKDV;

                ViewBag.KDV = basketitems.KDVPrice;

                ViewBag.TotalPrice = basketitems.TotalPrice;

                ViewBag.TotalPriceWithoutDiscount = basketitems.TotalPriceWithoutDiscount;
                ViewBag.DiscountRate = basketitems.DiscountRate;
                ViewBag.DiscountCode = basketitems.DiscountCode;
                
            }


            return View(count);
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketToItem(string ProductId, int Quantity = 1, string? selectedFilter = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.AddBasketItemToDatabase(ProductId, Quantity, selectedFilter);
            }
            else
            {
                await _basketService.AddBasketItemToCookies(ProductId, Quantity, selectedFilter);
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteBasket()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.DeleteBasketFromDatabase();
            }
            else
            {
                await _basketService.DeleteBasketFromCookies();
            }

            return RedirectToAction("Index");
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