using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Concretes;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using System.Net.Http;

namespace MultiShop.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderOderingService _orderOderingService;
        private readonly IOrderAddressService _orderAddressService;
        private readonly IUserService _userService;

    

        public OrderController(IUserService userService,IOrderOderingService orderOderingService, IOrderAddressService orderAddressService)
        {
            _orderOderingService = orderOderingService;
            _orderAddressService = orderAddressService;
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "MultiShop";
            ViewBag.directory2 = "Siparişler";
            ViewBag.directory3 = "Sipariş İşlemleri";
            return View();
        }


        public async Task<IActionResult> CreateOrderAndAdress(CreateOrderAddressDto createOrderAddressDto)
        {
            var values = await _userService.GetUserInfo();

            createOrderAddressDto.UserId = values.Id;
            createOrderAddressDto.Description = "aa";
            int adressId = await _orderAddressService.CreateOrderAddressAsync(createOrderAddressDto);

            await _orderOderingService.CreateOrdering(adressId);
            return RedirectToAction("Index", "Payment");
        }


        [HttpPost]
        public async Task<IActionResult> CreateJustOrder()
        {
            int adressId = 0; // bilinen adresten gelecek
            await _orderOderingService.CreateOrdering(adressId);

            return RedirectToAction("Index", "Payment");
        }


    }
}
