using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Services.FileUploadServices
{
    public interface IFileUploadService
    {
        Task<string?> UploadFileAsync(IFormFile? file, string folder = "uploads");
    }
}
