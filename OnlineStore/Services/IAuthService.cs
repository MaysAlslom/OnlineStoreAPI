using OnlineStoreAPI.Dto;
using YourProject.Models;

namespace OnlineStoreAPI.Services
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(RegisterDto registerDto);
        Task<ApplicationUser> LoginAsync(LoginDto loginDto);  // Now accepts LoginDto
        Task<string> GenerateJwtToken(ApplicationUser user);
        Task<bool> UserExists(string email);
    }
}
