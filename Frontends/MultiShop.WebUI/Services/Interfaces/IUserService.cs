using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailViewModel> GetUserInfo();
        Task<string> GetUserId();
        Task<bool> UpdateUserInfo(UserDetailViewModel userDetailViewModel);
        Task<(bool Success, string Message)> ChangePassword(string currentPassword, string newPassword);
    }
}

