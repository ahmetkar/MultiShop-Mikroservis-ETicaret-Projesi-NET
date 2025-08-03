using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.StatisticServices.CatalogStatisticsServices;
using MultiShop.WebUI.Services.StatisticServices.CommentStatisticServices;
using MultiShop.WebUI.Services.StatisticServices.UserStatisticsServices;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {

        private readonly ICatalogStatisticService _catalogStatisticService;
        private readonly IUserStatisticService _userStatisticService;
        private readonly ICommentStasticService _commentStasticService;
       

        public StatisticController(ICatalogStatisticService catalogStatisticService, IUserStatisticService userStatisticService,
            ICommentStasticService commentStasticService)
        {
            _catalogStatisticService = catalogStatisticService;
            _userStatisticService = userStatisticService;
            _commentStasticService = commentStasticService;
            
        }

        public async Task<IActionResult> Index()
        {
            var GetBrandCount = await _catalogStatisticService.GetBrandCount();
            ViewBag.BrandCount = GetBrandCount;

            var GetCategoryCount = await _catalogStatisticService.GetCategoryCount();
            ViewBag.CategoryCount = GetCategoryCount;

            var GetProductCount = await _catalogStatisticService.GetProductCount();
            ViewBag.ProductCount = GetProductCount;

            var GetMinPriceProductName = await _catalogStatisticService.GetMinPriceProductName();
            ViewBag.MinPriceProductName = GetMinPriceProductName;

            var GetMaxPriceProductName = await _catalogStatisticService.GetMaxPriceProductName();
            ViewBag.MaxPriceProductName = GetMaxPriceProductName;

            var GetProductAvgPrice = await _catalogStatisticService.GetProductAvgPrice();
            ViewBag.ProductAvgPrice = GetProductAvgPrice;

            var GetUserCount = await _userStatisticService.GetUserCount();
            ViewBag.UserCount = GetUserCount;


            var GetTotalCommentCount = await _commentStasticService.GetTotalCommentCount();
            ViewBag.TotalCommentCount = GetTotalCommentCount;

            var GetActiveCommentCount = await _commentStasticService.GetActiveCommentCount();
            ViewBag.ActiveCommentCount = GetActiveCommentCount;


            return View();
        }
    }
}
