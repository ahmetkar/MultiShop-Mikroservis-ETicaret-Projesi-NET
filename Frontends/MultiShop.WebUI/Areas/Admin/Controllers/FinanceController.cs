using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Finance")]
    public class FinanceController : Controller
    {
        private readonly IOrderOderingService _orderOderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICargoCompanyService _cargoCompanyService;

        public FinanceController(
            IOrderOderingService orderOderingService,
            IOrderDetailService orderDetailService,
            ICargoCompanyService cargoCompanyService)
        {
            _orderOderingService = orderOderingService;
            _orderDetailService = orderDetailService;
            _cargoCompanyService = cargoCompanyService;
        }

        void FinanceViewBag(string pagename)
        {
            ViewBag.v0 = "Kasa & Finans İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kasa Bilgileri";
            ViewBag.v3 = pagename;
        }

        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            FinanceViewBag("Kasa ve Gelir Raporu");

            var allOrders = await _orderOderingService.GetAllOrderingsAsync() ?? new List<ResultOrderingByUserIdDto>();
            var allDetails = await _orderDetailService.GetAllOrderDetailsAsync();

            decimal defaultCargoFee = 35m;
            var cargoCompanies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            if (cargoCompanies != null && cargoCompanies.Count > 0)
            {
                defaultCargoFee = cargoCompanies.First().CargoPrice;
            }

            // SADECE ve kesinlikle OrderStatus.Completed olan siparişler dahil edilir ve hesaplanır
            var completedOrders = allOrders.Where(x => x.Status == OrderStatus.Completed)
                                           .OrderByDescending(x => x.OrderDate)
                                           .ToList();

            decimal totalGross = completedOrders.Sum(x => x.TotalPrice);
            decimal totalCargo = completedOrders.Count * defaultCargoFee;
            decimal totalKDV = totalGross * 0.20m; // %20 standart KDV
            decimal netIncome = totalGross - totalKDV - totalCargo;
            if (netIncome < 0 && totalGross > 0) netIncome = totalGross - totalKDV;

            int totalSoldProducts = allDetails.Where(d => completedOrders.Select(o => o.OrderingId).Contains(d.OrderingId)).Sum(d => d.ProductAmount);

            ViewBag.TotalGrossRevenue = totalGross;
            ViewBag.TotalCargoExpense = totalCargo;
            ViewBag.TotalKDV = totalKDV;
            ViewBag.NetIncome = netIncome;
            ViewBag.CompletedOrdersCount = completedOrders.Count;
            ViewBag.TotalSoldProducts = totalSoldProducts;
            ViewBag.DefaultCargoFee = defaultCargoFee;
            ViewBag.AllDetails = allDetails;

            return View(completedOrders);
        }
    }
}
