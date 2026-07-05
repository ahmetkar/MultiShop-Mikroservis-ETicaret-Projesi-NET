using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Attributes;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Concretes;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using System.Net.Http;

namespace MultiShop.WebUI.Controllers
{
    [OrderAuthorize]
    public class OrderController : Controller
    {
        private readonly IOrderOderingService _orderOderingService;
        private readonly IOrderAddressService _orderAddressService;
        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IDataProtector _protector;



        public OrderController(IBasketService basketService,IUserService userService,IOrderOderingService orderOderingService, IOrderAddressService orderAddressService,IDataProtectionProvider provider)
        {
            _orderOderingService = orderOderingService;
            _orderAddressService = orderAddressService;
            _userService = userService;
            _basketService = basketService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");
        }


      
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.directory1 = "MultiShop";
            ViewBag.directory2 = "Siparişler";
            ViewBag.directory3 = "Sipariş İşlemleri";

            string myId = await _userService.GetUserId();

            var activeOrdering = await _orderOderingService.GetActiveOrderingByUserId(myId);

            if(activeOrdering !=null) {
                var encryptedId = _protector.Protect(activeOrdering.OrderingId.ToString());
                return RedirectToAction("Index","Payment", new {ActiveOrderingId = encryptedId}); 
            }

            int count = 0;
            var basket = await _basketService.GetBasketFromDatabase();

            count = basket.BasketItems.Count;

            
            if (count == 0) return RedirectToAction("Index","Default");


            var adressCount = await _orderAddressService.GetUserAdressCount();

            ViewBag.AdressCount = adressCount;

            ViewBag.CargoPrice = 15; // kargo fiyatını al
            

            return View();
            }


        [HttpPost]
        public async Task<IActionResult> CreateAdress(CreateAdressViewModel createAdressViewModel)
        {
            var count = 0;
            var userId = await _userService.GetUserId();
            var billingadress = createAdressViewModel.Billing;
            billingadress.UserId = userId;
            billingadress.IsBillingOrShipping = true;
            int billingAdressId = await _orderAddressService.CreateOrderAddressAsync(billingadress);

            if (billingAdressId != 0) {
                
                if (createAdressViewModel.IsShippingExists)
                {
                    var shippingadress = createAdressViewModel.Shipping;
                    shippingadress.UserId = userId;
                    shippingadress.IsBillingOrShipping = false;
                    int shippingAdressId = await _orderAddressService.CreateOrderAddressAsync(shippingadress);
                
                 }

            }

                return RedirectToAction("Index", "Order");
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(AdressListViewModel adressListViewModel)
        {
            try
            {
                int shippingAdressCount = await _orderAddressService.GetUserShippingAdressCount();

                int? orderingId = 0;
                if (shippingAdressCount > 0)
                {
                    orderingId = await _orderOderingService.CreateOrdering(adressListViewModel.BillingAdressId, adressListViewModel.ShippingAdressId);
                }
                else
                {
                    orderingId = await _orderOderingService.CreateOrdering(adressListViewModel.BillingAdressId, adressListViewModel.BillingAdressId);
                }

                if (orderingId!= null && orderingId != 0)
                {
                    var encryptedId = _protector.Protect(orderingId.ToString());
                    return RedirectToAction("Index", "Payment",new {ActiveOrderingId = encryptedId});
                }else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }

            }
            catch (Exception er)
            {
                return Content("Hata: " + er.Message);
            }
        }


    }
}
