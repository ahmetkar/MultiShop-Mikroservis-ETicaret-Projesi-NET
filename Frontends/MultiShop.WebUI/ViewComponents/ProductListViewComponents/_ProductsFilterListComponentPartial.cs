using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    //Her bir kategori için birden fazla filtre listesi çek ve ui'da göster.
    public class _ProductsFilterListComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(string categoryid)
        {
            return View();
        }
    }
}
