using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CategoriesDefaultViewComponentPartial : ViewComponent
    {
        private readonly IHttpService _httpService;
        public _CategoriesDefaultViewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CatalogApi");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpService.Get<ResultCategoryDto>("Categories");
            if (result != null) return View(result);
            return View();
        }
    }
}
