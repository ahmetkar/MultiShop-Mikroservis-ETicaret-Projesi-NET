using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/About")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;

        }


        void ViewBagList(string pagename)
        {
            ViewBag.v0 = "Hakkımızda Alanı İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Hakkımızda Alanı";
            ViewBag.v3 = pagename;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            ViewBagList("Hakkımızda Alanı Listesi");

            var result = await _aboutService.GetAbout();
            if (result != null) return View(result);
            return View();
        }


        [Route("UpdateAbout")]
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(ResultAboutDto res)
        {
            UpdateAboutDto updateAboutDto = new UpdateAboutDto
             {
                AboutId = res.AboutId,
                Address = res.Address,
                Description = res.Description,
                Email = res.Email,
                Phone = res.Phone,
            };
            await _aboutService.UpdateAboutAsync(updateAboutDto);   
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }
    }
}
