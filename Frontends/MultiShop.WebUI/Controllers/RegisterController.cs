using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.RegisterDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class RegisterController : Controller
    {

        private readonly IHttpService _httpService;
        public RegisterController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("IdentityApi");
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {

            if (createRegisterDto.Password == createRegisterDto.ConfirmPassword)
            {
                var result = await _httpService.Create<CreateRegisterDto>("Registers", createRegisterDto);

                if (result) { return RedirectToAction("Index", "Login"); }
            }
            return View();

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
