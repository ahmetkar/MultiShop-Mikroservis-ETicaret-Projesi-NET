using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;

namespace MultiShop.WebUI.ViewComponents.OrderViewComponents
{

        public class _AdressListViewComponentPartial : ViewComponent
        {

        private readonly IOrderAddressService _orderAddressService;

        public _AdressListViewComponentPartial(IOrderAddressService orderAddressService)
        {
            _orderAddressService = orderAddressService;
        }

        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var adresses = await _orderAddressService.GetUserAddressesByUserIdAsync();
            var billingAdressCount = await _orderAddressService.GetUserBillingAdressCount();
            var shippingAdressCount = await _orderAddressService.GetUserShippingAdressCount();

            return View(new AdressListViewModel() { resultOrderAddressDto = adresses,BillingAdressCount = billingAdressCount,ShippingAdressCount = shippingAdressCount});
         }
        }
    }

