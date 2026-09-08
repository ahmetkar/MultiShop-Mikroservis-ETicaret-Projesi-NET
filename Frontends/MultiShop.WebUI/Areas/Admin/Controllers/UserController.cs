using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.UserDtos;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.CargoServices.CargoCustomerServices;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.UserIdentityServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserIdentityService _userIdentityService;
        private readonly ICargoCustomerService _cargoCustomerService;
        private readonly IOrderAddressService _orderAddressService;

        public UserController(
            IUserIdentityService userIdentityService,
            ICargoCustomerService cargoCustomerService,
            IOrderAddressService orderAddressService)
        {
            _userIdentityService = userIdentityService;
            _cargoCustomerService = cargoCustomerService;
            _orderAddressService = orderAddressService;
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _userIdentityService.GetAllUserListAsync();
            return View(values);
        }

        public async Task<IActionResult> UserAddressInfo(string id)
        {
            var cargoCustomer = await _cargoCustomerService.GetByIdCargoCustomerInfoAsync(id);
            var orderAddresses = await _orderAddressService.GetAddressesByExplicitUserIdAsync(id);
            var user = await _userIdentityService.GetUserByIdAsync(id);

            ViewBag.User = user;
            ViewBag.OrderAddresses = orderAddresses ?? new List<ResultOrderAddressDto>();
            ViewBag.CargoCustomer = cargoCustomer;

            return View(cargoCustomer);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(AdminUpdateUserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Id))
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı bilgisi.";
                return RedirectToAction("UserList");
            }

            var result = await _userIdentityService.UpdateUserAsync(dto);
            if (result)
            {
                TempData["SuccessMessage"] = "Kullanıcı bilgileri başarıyla güncellendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
            }

            return RedirectToAction("UserAddressInfo", new { id = dto.Id });
        }

        [HttpGet("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userIdentityService.DeleteUserAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu.";
            }

            return RedirectToAction("UserList");
        }
    }
}
