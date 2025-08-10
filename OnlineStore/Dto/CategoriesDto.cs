using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI.Dto
{
    public class CategoriesDto
    {
        [Required]
        public string Name { get; set; }
    }

}
