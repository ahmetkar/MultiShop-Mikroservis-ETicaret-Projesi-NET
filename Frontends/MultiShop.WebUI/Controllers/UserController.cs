using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.LoginDtos;
using MultiShop.WebUI.Services.CargoServices.CargoCustomerServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoginService _loginService;


        public UserController(IUserService userService, IIdentityService identityService, IHttpClientFactory httpClientFactory, ILoginService loginService)
        {
            _userService = userService;
            _identityService = identityService;
            _httpClientFactory = httpClientFactory;
            _loginService = loginService;
            _identityService = identityService;
        }


        public async  Task<IActionResult> Index()
        {
            var values = await _userService.GetUserInfo();
            return View(values);
        }

        public async Task<IActionResult> Logout()
        {
            await _identityService.SignOut();
            return RedirectToAction("Index","Default");
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInDto signInDto)
        {
            await _identityService.SignIn(signInDto);
            return RedirectToAction("Index", "User");
        }


    }
}
