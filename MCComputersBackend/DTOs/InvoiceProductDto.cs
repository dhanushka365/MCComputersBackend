using System.ComponentModel.DataAnnotations;

namespace MCComputersBackend.DTOs
{
    public class InvoiceProductDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive integer")]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }
    }
}
