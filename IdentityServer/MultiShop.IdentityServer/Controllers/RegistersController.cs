using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.IdentityServer.DTOs;
using MultiShop.IdentityServer.Models;
using System.Threading.Tasks;

namespace MultiShop.IdentityServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null)
            {
                return BadRequest(new[] { new { Code = "NullRequest", Description = "Kayıt bilgileri boş gönderilemez." } });
            }

            if (string.IsNullOrWhiteSpace(userRegisterDto.UserName))
            {
                return BadRequest(new[] { new { Code = "EmptyUserName", Description = "Kullanıcı adı boş bırakılamaz." } });
            }

            if (string.IsNullOrWhiteSpace(userRegisterDto.Email))
            {
                return BadRequest(new[] { new { Code = "EmptyEmail", Description = "E-posta adresi boş bırakılamaz." } });
            }

            if (string.IsNullOrWhiteSpace(userRegisterDto.Password))
            {
                return BadRequest(new[] { new { Code = "EmptyPassword", Description = "Şifre boş bırakılamaz." } });
            }

            // Ensure default roles exist
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var values = new ApplicationUser()
            {
                UserName = userRegisterDto.UserName.Trim(),
                Email = userRegisterDto.Email.Trim(),
                Name = userRegisterDto.Name?.Trim() ?? "",
                Surname = userRegisterDto.Surname?.Trim() ?? "",
            };

            var result = await _userManager.CreateAsync(values, userRegisterDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(values, "User");
                return Ok(new { success = true, message = "Kullanıcı başarıyla kaydedildi." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}

