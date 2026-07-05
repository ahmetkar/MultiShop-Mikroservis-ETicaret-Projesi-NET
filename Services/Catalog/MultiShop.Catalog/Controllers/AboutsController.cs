using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.DTOs.AboutDTOs;
using MultiShop.Catalog.Services.AboutServices;
using System.Formats.Asn1;

namespace MultiShop.Catalog.Controllers

{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        public AboutsController(IAboutService AboutService) {
     
            _aboutService = AboutService;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAbout() {
            var value = await _aboutService.GetAbout();
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            await _aboutService.UpdateAboutAsync(updateAboutDto);
            return Ok("Hakkımızda Alanı başarıyla güncellendi.");
        }
    }
}
