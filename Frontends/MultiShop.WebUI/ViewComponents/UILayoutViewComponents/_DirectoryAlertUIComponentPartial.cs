using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _DirectoryAlertUIComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return  View();
        }
    }
}
