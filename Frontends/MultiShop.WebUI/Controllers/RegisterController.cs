using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.RegisterDtos;
using MultiShop.WebUI.Services.Interfaces;
using System.Net.Http;

namespace MultiShop.WebUI.Controllers
{
    public class RegisterController : Controller
    {

        private readonly HttpClient _httpClient;

        public RegisterController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {

            if (createRegisterDto.Password == createRegisterDto.ConfirmPassword)
            {
                var result = await _httpClient.PostAsJsonAsync("Registers", createRegisterDto);

                if (result.IsSuccessStatusCode) { return RedirectToAction("Index", "Login"); }
            }
            return View();

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
