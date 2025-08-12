using OnlineStoreAPI.Dto;
using YourProject.Models;
using System.Threading.Tasks;
using OnlineStoreAPI.Models;

namespace OnlineStoreAPI.Services
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(RegisterDto registerDto);
        Task<ApplicationUser> LoginAsync(LoginDto loginDto);
        Task<string> GenerateJwtToken(ApplicationUser user);
        Task<bool> UserExists(string email);
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<bool> UserHasRole(ApplicationUser user, string role);  // Add this method
    }

}