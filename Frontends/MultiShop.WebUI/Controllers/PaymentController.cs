using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.DtoLayer.PaymentDtos;
using MultiShop.WebUI.Attributes;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using MultiShop.WebUI.Services.PaymentServices;

namespace MultiShop.WebUI.Controllers
{
    [OrderAuthorize]
    public class PaymentController : Controller
    {

        private readonly IBasketService _basketService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly IUserService _userService;
        private readonly IDataProtector _protector;
        private readonly IPaymentService _paymentService;

        public PaymentController(IBasketService basketService, IOrderOderingService orderOderingService, IUserService userService, IDataProtectionProvider provider, IPaymentService paymentService)
        {

            _basketService = basketService;
            _orderOderingService = orderOderingService;
            _userService = userService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");
            _paymentService = paymentService;

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


            if (count == 0) return RedirectToAction("Index", "Default");



            return View(paymentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendPayment(CreatePaymentViewModel createPaymentViewModel)
        {

            string[] ownernamesurname = createPaymentViewModel.OwnerNameAndSurname.Split(' ');
            string ownername = ownernamesurname[0];
            string ownersurname = ownernamesurname[1];

            var orderinginfo = await _orderOderingService.GetOrderingById(createPaymentViewModel.OrderingId);

            CreatePaymentDto createPayment = new CreatePaymentDto
            {
                LastFourNumber = createPaymentViewModel.LastFourNumber,
                LastDateMonth = createPaymentViewModel.LastDateMonth,
                LastDateYear = createPaymentViewModel.LastDateYear,
                OwnerName = ownername,
                OwnerSurname = ownersurname,
                OrderingId = createPaymentViewModel.OrderingId,
                PaymentType = "creditcard",
                CardType = createPaymentViewModel.CardType,
                CardBankName = "BankA",
                CardBrand = "BrandA",
                PaymentTotal = (int)orderinginfo.TotalPrice,
                UserId = orderinginfo.UserId
            };

            bool added = await _paymentService.AddPayment(createPayment);

            ViewBag.PaymentResult = added;

            return RedirectToAction("PaymentResult", "Payment");

        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(CreatePaymentViewModel createPaymentViewModel)
        {
    
            
            if(createPaymentViewModel.OrderingId != 0)
            {
                bool delete = await _orderOderingService.DeleteOrdering(createPaymentViewModel.OrderingId);

                if (delete)
                {
                    return RedirectToAction("Index", "Default");
                }
            }
            
            return RedirectToAction("Index","Payment");
        }

        [HttpGet]
        public  IActionResult PaymentResult()
        {
            return View();
        }
    }
}
