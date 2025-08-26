using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Attributes;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;

namespace MultiShop.WebUI.Controllers
{
    [OrderAuthorize]
    public class PaymentController : Controller
    {
       
        private readonly IBasketService _basketService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly IUserService _userService;
        private readonly IDataProtector _protector;

        public PaymentController(IBasketService basketService, IOrderOderingService orderOderingService, IUserService userService, IDataProtectionProvider provider)
        {
         
            _basketService = basketService;
            _orderOderingService = orderOderingService;
            _userService = userService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");

        }

        [HttpGet]
        public async Task<IActionResult> Index(string ActiveOrderingId)
        {
            CreatePaymentViewModel paymentViewModel = new CreatePaymentViewModel();
            
                try
                {
                    var decrypted = _protector.Unprotect(ActiveOrderingId);
                    int activeOrderingId = int.Parse(decrypted);

                if (activeOrderingId != 0)
                {
                    string myId = await _userService.GetUserId();
                    var activeOrdering = await _orderOderingService.GetActiveOrderingByUserId(myId);
                    if (activeOrdering != null)
                    {
                        if (activeOrdering.OrderingId == activeOrderingId)
                        {
                            paymentViewModel.OrderingId = activeOrdering.OrderingId;
                            paymentViewModel.PaymentTotal = activeOrdering.TotalPrice;
                            paymentViewModel.UserId = myId;
                        }
                    }
                }
                }
                catch
                {
                return RedirectToAction("Index", "Default");
                }

            

            int count = 0;
            var basket = await _basketService.GetBasketFromDatabase();


            count = basket.BasketItems.Count;

            
            if (count == 0) return RedirectToAction("Index","Default");

            

            return View(paymentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendPayment(CreatePaymentViewModel createPaymentViewModel)
        {


            return View();
            }

        [HttpPost]
        public async Task<IActionResult> CancelPayment(CreatePaymentViewModel createPaymentViewModel)
        {


            return View();
        }
    }
}
