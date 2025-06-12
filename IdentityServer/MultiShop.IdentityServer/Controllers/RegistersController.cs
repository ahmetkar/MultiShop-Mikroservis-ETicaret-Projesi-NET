using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.IdentityServer.DTOs;
using MultiShop.IdentityServer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MultiShop.IdentityServer.Controllers
{
    //[Authorize(IdentityServer4.IdentityServerConstants.LocalApi.PolicyName)]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegistersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto)
        {
            var values = new ApplicationUser()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
            };
            var result = await _userManager.CreateAsync(values, userRegisterDto.Password);

            if (result.Succeeded)
            {
                return Ok("Kullanıcı başarıyla eklendi.");
            }
            else
            {
                var errors = result.Errors.ToList();

                var errorstring = "";
                foreach(var error in errors) errorstring+="Hatalar : "+error.Description + "\n";
                return Ok("Hata oluştu."+errorstring);
            }
        }






    }
}
