using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI.Dto
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }

}
