using MultiShop.DtoLayer.IdentityDtos.UserDtos;

namespace MultiShop.WebUI.Services.UserIdentityServices
{
    public interface IUserIdentityService
    {
        Task<List<ResultUserDto>> GetAllUserListAsync();
        Task<ResultUserDto?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(AdminUpdateUserDto dto);
        Task<bool> DeleteUserAsync(string id);
    }
}
