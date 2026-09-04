using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;
using MultiShop.DtoLayer.CargoDtos.CargoOperationDtos;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.WebUI.Services.CargoServices.CargoDetailServices;
using MultiShop.WebUI.Services.CargoServices.CargoOperationServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    [Route("UserProfile")]
    [Route("User/Index")]
    [Route("User/Profile")]
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderAddressService _orderAddressService;
        private readonly ICargoOperationService _cargoOperationService;
        private readonly ICargoDetailService _cargoDetailService;
        private readonly ICargoCompanyService _cargoCompanyService;
        private readonly IDataProtector _protector;

        public UserProfileController(
            IUserService userService,
            IOrderOderingService orderOderingService,
            IOrderDetailService orderDetailService,
            IOrderAddressService orderAddressService,
            ICargoOperationService cargoOperationService,
            ICargoDetailService cargoDetailService,
            ICargoCompanyService cargoCompanyService,
            IDataProtectionProvider provider)
        {
            _userService = userService;
            _orderOderingService = orderOderingService;
            _orderDetailService = orderDetailService;
            _orderAddressService = orderAddressService;
            _cargoOperationService = cargoOperationService;
            _cargoDetailService = cargoDetailService;
            _cargoCompanyService = cargoCompanyService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string activeTab = "orders")
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Hesabım";
            ViewBag.Directory3 = "Kullanıcı Profili";
            ViewBag.ActiveTab = activeTab;

            var userInfo = await _userService.GetUserInfo();
            ViewBag.UserInfo = userInfo;

            var userOrders = await _orderOderingService.GetOrderingByUserId(userInfo.Id);
            ViewBag.UserOrders = userOrders ?? new List<ResultOrderingByUserIdDto>();

            var allDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            ViewBag.AllOrderDetails = allDetails ?? new List<ResultOrderDetailDto>();

            var addresses = await _orderAddressService.GetUserAddressesByUserIdAsync();
            ViewBag.Addresses = addresses ?? new List<ResultOrderAddressDto>();

            var cargoOperations = await _cargoOperationService.GetAllCargoOperationsAsync();
            ViewBag.CargoOperations = cargoOperations ?? new List<ResultCargoOperationDto>();

            var cargoDetails = await _cargoDetailService.GetAllCargoDetailsAsync();
            ViewBag.CargoDetails = cargoDetails ?? new List<ResultCargoDetailDto>();

            var cargoCompanies = await _cargoCompanyService.GetAllCargoCompanyAsync();
            ViewBag.CargoCompanies = cargoCompanies ?? new List<MultiShop.DtoLayer.CargoDtos.CargoCompanyDtos.ResultCargoCompanyDto>();

            return View(userInfo);
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _userService.UpdateUserInfo(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Profil bilgileri güncellenirken bir hata oluştu.";
                }
            }
            return RedirectToAction("Index", new { activeTab = "settings" });
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                TempData["ErrorMessage"] = "Mevcut şifre ve yeni şifre alanları boş bırakılamaz.";
                return RedirectToAction("Index", new { activeTab = "settings" });
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                TempData["ErrorMessage"] = "Girdiğiniz yeni şifreler birbiriyle eşleşmiyor.";
                return RedirectToAction("Index", new { activeTab = "settings" });
            }

            if (model.NewPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "Yeni şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("Index", new { activeTab = "settings" });
            }

            var (success, message) = await _userService.ChangePassword(model.CurrentPassword, model.NewPassword);
            if (success)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction("Index", new { activeTab = "settings" });
        }

        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateAddress(CreateOrderAddressDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Surname) || string.IsNullOrWhiteSpace(dto.City) || string.IsNullOrWhiteSpace(dto.District) || string.IsNullOrWhiteSpace(dto.Detail1))
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu adres alanlarını (Ad, Soyad, Şehir, İlçe, Adres) doldurunuz.";
                return RedirectToAction("Index", new { activeTab = "addresses" });
            }

            var addressId = await _orderAddressService.CreateOrderAddressAsync(dto);
            if (addressId > 0)
            {
                TempData["SuccessMessage"] = "Yeni adresiniz başarıyla kaydedildi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Adres kaydedilirken bir hata oluştu.";
            }

            return RedirectToAction("Index", new { activeTab = "addresses" });
        }

        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(UpdateOrderAddressDto dto)
        {
            if (dto.AdressId <= 0 || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Surname) || string.IsNullOrWhiteSpace(dto.City) || string.IsNullOrWhiteSpace(dto.District) || string.IsNullOrWhiteSpace(dto.Detail1))
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu adres alanlarını doldurunuz.";
                return RedirectToAction("Index", new { activeTab = "addresses" });
            }

            try
            {
                await _orderAddressService.UpdateOrderAddressAsync(dto);
                TempData["SuccessMessage"] = "Adresiniz başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Adres güncellenirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index", new { activeTab = "addresses" });
        }

        [HttpGet("DeleteAddress/{id}")]
        [HttpPost("DeleteAddress/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                await _orderAddressService.DeleteOrderAddressAsync(id);
                TempData["SuccessMessage"] = "Adres başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Adres silinirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index", new { activeTab = "addresses" });
        }

        [HttpGet("ConfirmDelivery/{orderingId}")]
        [HttpPost("ConfirmDelivery/{orderingId}")]
        public async Task<IActionResult> ConfirmDelivery(int orderingId)
        {
            // 1. Set cargo operation isdelivered / completed
            var cargoOperations = await _cargoOperationService.GetAllCargoOperationsAsync();
            var op = cargoOperations.FirstOrDefault(x => x.OrderingId == orderingId);
            if (op != null)
            {
                await _cargoOperationService.ConfirmDeliveryAsync(op.CargoOperationId);
            }

            // 2. Set order status to Completed
            await _orderOderingService.UpdateOrderStatusAsync(orderingId, OrderStatus.Completed);

            TempData["SuccessMessage"] = $"Sipariş #{orderingId} teslim alındı olarak onaylandı.";
            return RedirectToAction("Index", new { activeTab = "orders" });
        }

        [HttpGet("ContinuePayment/{orderingId}")]
        public IActionResult ContinuePayment(int orderingId)
        {
            var encryptedId = _protector.Protect(orderingId.ToString());
            return RedirectToAction("Index", "Payment", new { ActiveOrderingId = encryptedId });
        }

        [HttpGet("CancelOrder/{orderingId}")]
        [HttpPost("CancelOrder/{orderingId}")]
        public async Task<IActionResult> CancelOrder(int orderingId)
        {
            // Set order status to Cancelled
            await _orderOderingService.UpdateOrderStatusAsync(orderingId, OrderStatus.Cancelled);

            TempData["SuccessMessage"] = $"Sipariş #{orderingId} iptal edildi.";
            return RedirectToAction("Index", new { activeTab = "orders" });
        }
    }
}
