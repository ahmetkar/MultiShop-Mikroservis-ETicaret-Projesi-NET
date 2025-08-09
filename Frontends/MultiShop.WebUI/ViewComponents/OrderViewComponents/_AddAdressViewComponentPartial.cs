using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.ViewComponents.OrderViewComponents
{
    public class _AddAdressViewComponentPartial : ViewComponent
    {

       

        public _AddAdressViewComponentPartial()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View(new CreateAdressViewModel());
        }
    }

}