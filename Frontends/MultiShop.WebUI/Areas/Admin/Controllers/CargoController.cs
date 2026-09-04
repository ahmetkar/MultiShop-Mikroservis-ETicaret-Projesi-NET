using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CargoDtos.CargoCompanyDtos;
using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;
using MultiShop.DtoLayer.CargoDtos.CargoOperationDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.WebUI.Services.CargoServices.CargoDetailServices;
using MultiShop.WebUI.Services.CargoServices.CargoOperationServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Cargo")]
    public class CargoController : Controller
    {
        private readonly ICargoCompanyService _cargoCompanyService;
        private readonly ICargoDetailService _cargoDetailService;
        private readonly ICargoOperationService _cargoOperationService;
        private readonly IOrderOderingService _orderOderingService;

        public CargoController(
            ICargoCompanyService cargoCompanyService,
            ICargoDetailService cargoDetailService,
            ICargoOperationService cargoOperationService,
            IOrderOderingService orderOderingService)
        {
            _cargoCompanyService = cargoCompanyService;
            _cargoDetailService = cargoDetailService;
            _cargoOperationService = cargoOperationService;
            _orderOderingService = orderOderingService;
        }

        [Route("CargoCompanyList")]
        public async Task<IActionResult> CargoCompanyList()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kargo Şirketleri";
            ViewBag.v3 = "Kargo Şirket Listesi";
            ViewBag.v0 = "Kargo İşlemleri";
            var values = await _cargoCompanyService.GetAllCargoCompanyAsync();
            return View(values);
        }

        [HttpGet]
        [Route("CreateCargoCompany")]
        public async Task<IActionResult> CreateCargoCompany()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kargo Şirketleri";
            ViewBag.v3 = "Yeni Kargo Şirketi Ekle";
            ViewBag.v0 = "Kargo İşlemleri";
            return View();
        }

        [HttpPost]
        [Route("CreateCargoCompany")]
        public async Task<IActionResult> CreateCargoCompany(CreateCargoCompanyDto createCargoCompanyDto)
        {
            await _cargoCompanyService.CreateCargoCompanyAsync(createCargoCompanyDto);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }

        [Route("DeleteCargoCompany/{id}")]
        public async Task<IActionResult> DeleteCargoCompany(int id)
        {
            await _cargoCompanyService.DeleteCargoCompanyAsync(id);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }

        [HttpGet]
        [Route("UpdateCargoCompany/{id}")]
        public async Task<IActionResult> UpdateCargoCompany(int id)
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kargo Şirketleri";
            ViewBag.v3 = "Kargo Şirketi Güncelle";
            ViewBag.v0 = "Kargo İşlemleri";
            var values = await _cargoCompanyService.GetByIdCargoCompany(id);
            return View(values);
        }

        [HttpPost]
        [Route("UpdateCargoCompany/{id}")]
        [Route("UpdateCargoCompany")]
        public async Task<IActionResult> UpdateCargoCompany(UpdateCargoCompanyDto updateCargoCompanyDto)
        {
            await _cargoCompanyService.UpdateCargoCompanyAsync(updateCargoCompanyDto);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Admin" });
        }

        [Route("CargoProcess")]
        public async Task<IActionResult> CargoProcess()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Kargo Süreçleri";
            ViewBag.v3 = "Kargo Takip & Süreç Yönetimi";
            ViewBag.v0 = "Kargo İşlemleri";

            var operations = await _cargoOperationService.GetAllCargoOperationsAsync();
            var details = await _cargoDetailService.GetAllCargoDetailsAsync();
            var companies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            var orders = await _orderOderingService.GetAllOrderingsAsync();

            ViewBag.Details = details;
            ViewBag.Companies = companies;
            ViewBag.Orders = orders;

            return View(operations);
        }

        [HttpGet]
        [Route("ConfirmDelivery/{id}")]
        public async Task<IActionResult> ConfirmDelivery(int id)
        {
            var op = await _cargoOperationService.GetByIdCargoOperationAsync(id);
            if (op != null && op.OrderingId > 0)
            {
                // Kargo tablosundaki durum değişmez, ilişkili order tablosunda sipariş durumu CargoApproved olur
                await _orderOderingService.UpdateOrderStatusAsync(op.OrderingId, OrderStatus.CargoApproved);
            }
            return RedirectToAction("CargoProcess");
        }
    }
}


