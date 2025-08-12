using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI.Dto
{
    public class ProfileDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
