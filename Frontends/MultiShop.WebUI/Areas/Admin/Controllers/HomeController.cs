using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.CargoServices.CargoOperationServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using MultiShop.WebUI.Services.StatisticServices.CatalogStatisticsServices;
using MultiShop.WebUI.Services.StatisticServices.CommentStatisticServices;
using MultiShop.WebUI.Services.StatisticServices.UserStatisticsServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/Home")]
    public class HomeController : Controller
    {
        private readonly ICatalogStatisticService _catalogStatisticService;
        private readonly IUserStatisticService _userStatisticService;
        private readonly ICommentStasticService _commentStasticService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly ICargoOperationService _cargoOperationService;

        public HomeController(
            ICatalogStatisticService catalogStatisticService,
            IUserStatisticService userStatisticService,
            ICommentStasticService commentStasticService,
            IOrderOderingService orderOderingService,
            ICargoOperationService cargoOperationService)
        {
            _catalogStatisticService = catalogStatisticService;
            _userStatisticService = userStatisticService;
            _commentStasticService = commentStasticService;
            _orderOderingService = orderOderingService;
            _cargoOperationService = cargoOperationService;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Admin Paneli";
            ViewBag.v2 = "Kontrol Paneli";
            ViewBag.v3 = "Genel İstatistikler & Özet";
            ViewBag.v0 = "Dashboard";

            try
            {
                ViewBag.BrandCount = await _catalogStatisticService.GetBrandCount();
                ViewBag.CategoryCount = await _catalogStatisticService.GetCategoryCount();
                ViewBag.ProductCount = await _catalogStatisticService.GetProductCount();
                ViewBag.ProductAvgPrice = await _catalogStatisticService.GetProductAvgPrice();
                ViewBag.MinPriceProductName = await _catalogStatisticService.GetMinPriceProductName();
                ViewBag.MaxPriceProductName = await _catalogStatisticService.GetMaxPriceProductName();
            }
            catch
            {
                ViewBag.BrandCount = 0;
                ViewBag.CategoryCount = 0;
                ViewBag.ProductCount = 0;
                ViewBag.ProductAvgPrice = 0m;
                ViewBag.MinPriceProductName = "-";
                ViewBag.MaxPriceProductName = "-";
            }

            try
            {
                ViewBag.UserCount = await _userStatisticService.GetUserCount();
            }
            catch
            {
                ViewBag.UserCount = 0;
            }

            try
            {
                ViewBag.TotalCommentCount = await _commentStasticService.GetTotalCommentCount();
                ViewBag.ActiveCommentCount = await _commentStasticService.GetActiveCommentCount();
            }
            catch
            {
                ViewBag.TotalCommentCount = 0;
                ViewBag.ActiveCommentCount = 0;
            }

            try
            {
                var allOrders = await _orderOderingService.GetAllOrderingsAsync();
                ViewBag.TotalOrdersCount = allOrders?.Count ?? 0;
                ViewBag.CompletedOrdersCount = allOrders?.Count(x => x.Status == OrderStatus.Completed) ?? 0;
                ViewBag.TotalGrossRevenue = allOrders?.Where(x => x.Status == OrderStatus.Completed).Sum(x => x.TotalPrice) ?? 0m;
                ViewBag.RecentOrders = allOrders?.OrderByDescending(x => x.OrderDate).Take(5).ToList() ?? new List<ResultOrderingByUserIdDto>();
            }
            catch
            {
                ViewBag.TotalOrdersCount = 0;
                ViewBag.CompletedOrdersCount = 0;
                ViewBag.TotalGrossRevenue = 0m;
                ViewBag.RecentOrders = new List<ResultOrderingByUserIdDto>();
            }

            try
            {
                var allCargos = await _cargoOperationService.GetAllCargoOperationsAsync();
                ViewBag.TotalCargoCount = allCargos?.Count ?? 0;
            }
            catch
            {
                ViewBag.TotalCargoCount = 0;
            }

            return View();
        }
    }
}
