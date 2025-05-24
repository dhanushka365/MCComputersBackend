using System.ComponentModel.DataAnnotations;

namespace MCComputersBackend.DTOs
{
    public class UpdateProductDto
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative")]
        public int? StockQuantity { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public IFormFile? Image { get; set; }
    }
}
