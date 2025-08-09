using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.LoginDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CargoServices.CargoCustomerServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _contextAccessor;
       

        public AuthController(IUserService userService, IIdentityService identityService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _identityService = identityService;
            _contextAccessor = contextAccessor;
        }


        public async Task<IActionResult> Index()
        {
            var values = await _userService.GetUserInfo();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _identityService.SignOut();
            _contextAccessor.HttpContext.Session.SetInt32("IsCookiesAdded", 0);
            return RedirectToAction("Index","Default");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

    

        [HttpPost]
        public async Task<IActionResult> Login(SignInDto signInDto)
        {
            await _identityService.SignIn(signInDto);
          
            return RedirectToAction("Index", "Default");
        }


    }
}
