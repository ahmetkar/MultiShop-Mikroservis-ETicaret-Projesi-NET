using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.LoginDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IIdentityService identityService,
            IHttpContextAccessor contextAccessor,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _identityService = identityService;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _identityService.SignOut();
            _contextAccessor.HttpContext?.Session?.SetInt32("IsCookiesAdded", 0);
            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInDto signInDto, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(signInDto);
            }

            try
            {
                var result = await _identityService.SignIn(signInDto);
                if (!result)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı. Lütfen bilgilerinizi kontrol ediniz.");
                    return View(signInDto);
                }

                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // Check role for redirection: Admin/Manager -> Admin Area, User -> Home
                if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Default");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Giriş sırasında kimlik sunucusuna bağlanılamadı.");
                ModelState.AddModelError("", "Kimlik doğrulama sunucusuna bağlanılamadı. Lütfen sunucunun açık olduğundan emin olunuz.");
                return View(signInDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında beklenmeyen hata oluştu.");
                ModelState.AddModelError("", "Giriş işlemi sırasında bir hata oluştu: " + ex.Message);
                return View(signInDto);
            }
        }
    }
}
