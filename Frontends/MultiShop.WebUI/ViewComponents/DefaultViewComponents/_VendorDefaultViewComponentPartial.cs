using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.BrandDtos;
using MultiShop.WebUI.Services.CatalogServices.BrandServices;


namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _VendorDefaultViewComponentPartial : ViewComponent
    {

        private readonly IBrandService _brandService;
        public _VendorDefaultViewComponentPartial(IBrandService brandService)
        {
            _brandService = brandService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _brandService.GetAllBrandAsync();
            return View(result);
        }
    }
}
