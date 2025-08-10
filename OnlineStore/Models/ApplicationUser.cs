using Microsoft.AspNetCore.Identity;

namespace YourProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties if needed
        public string FullName { get; set; }
    }
}

