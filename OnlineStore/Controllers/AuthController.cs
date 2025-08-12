using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineStoreAPI.Dto;  // Make sure the LoginDto is here
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YourProject.Models;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _authService.UserExists(registerDto.Email))
                return BadRequest("Email already exists");

            var user = await _authService.RegisterAsync(registerDto);
            if (user == null)
                return BadRequest("Registration failed");

            return Ok(new { Message = "User registered successfully" });
        }




        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validate the user credentials using the service method
            var user = await _authService.LoginAsync(loginDto);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // Generate JWT token after successful login
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }


        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginDto loginDto)
        {
            var user = await _authService.LoginAsync(loginDto); // Pass the LoginDto directly

            if (user == null || !await _authService.UserHasRole(user, "Admin"))
                return Unauthorized("Invalid credentials or not an admin.");

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }




        private string GenerateJwtToken(ApplicationUser user)
        {
            // Create JWT token logic (example)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey")); // Make sure to replace with your secret key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

