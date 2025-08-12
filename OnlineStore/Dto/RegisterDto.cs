using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI.Dto
{
    public class RegisterDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$",
            ErrorMessage = "Password must be at least 6 characters, contain a number and a special character.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

}
