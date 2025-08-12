using System.ComponentModel.DataAnnotations;

namespace OnlineStoreAPI.Dto
{
    public class ProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be greater than or equal to 0.")]
        public int StockQuantity { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Description { get; set; }
    }


}
