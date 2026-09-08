using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.IdentityServer.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MultiShop.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userClaim = User.Claims.FirstOrDefault(x=>x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userClaim.Value);
            return Ok(new {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email =user.Email,
                UserName = user.UserName
            });
        }

        [HttpGet("GetUserId")]
        public async Task<IActionResult> GetUserId()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userClaim.Value);
            return Ok(user.Id);
        }

        [HttpGet("GetAllUserList")]
        public async Task<IActionResult> GetAllUserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");
            return Ok(user);
        }

        [HttpPost("AdminUpdateUser")]
        public async Task<IActionResult> AdminUpdateUser([FromBody] AdminUserUpdateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Id)) return BadRequest("Geçersiz kullanıcı verisi.");
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            user.Name = dto.Name ?? user.Name;
            user.Surname = dto.Surname ?? user.Surname;
            user.Email = dto.Email ?? user.Email;
            user.UserName = dto.UserName ?? user.UserName;
            user.PhoneNumber = dto.PhoneNumber;
            user.LockoutEnabled = dto.LockoutEnabled;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Kullanıcı başarıyla güncellendi." });
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Kullanıcı başarıyla silindi." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserUpdateDto dto)
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userClaim == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userClaim.Value);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.UserName = dto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Kullanıcı bilgileri başarıyla güncellendi." });
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto dto)
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub || x.Type == "sub" || x.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userClaim.Value);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return BadRequest(new[] { new { Code = "EmptyFields", Description = "Mevcut şifre ve yeni şifre boş bırakılamaz." } });
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = "Şifreniz başarıyla güncellendi." });
            }

            return BadRequest(result.Errors);
        }
    }

    public class UserChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class AdminUserUpdateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
