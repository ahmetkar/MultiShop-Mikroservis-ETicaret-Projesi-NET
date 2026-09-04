using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;

namespace MultiShop.WebUI.ViewComponents.OrderViewComponents
{
    public class _OrderSummaryComponentPartial : ViewComponent
    {
        private readonly IBasketService _basketService;
        private readonly ICargoCompanyService _cargoCompanyService;

        public _OrderSummaryComponentPartial(IBasketService basketService, ICargoCompanyService cargoCompanyService)
        {
            _basketService = basketService;
            _cargoCompanyService = cargoCompanyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basketTotal = await _basketService.GetBasketFromDatabase();

            decimal cargoPrice = 35;
            var companies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            if (companies != null && companies.Count > 0)
            {
                cargoPrice = companies.First().CargoPrice;
            }
            ViewBag.CargoPrice = cargoPrice;

            return View(basketTotal);
        }
    }
}

