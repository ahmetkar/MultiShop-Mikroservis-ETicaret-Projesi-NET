using Microsoft.AspNetCore.Mvc;

namespace MultiShop.RapidApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
