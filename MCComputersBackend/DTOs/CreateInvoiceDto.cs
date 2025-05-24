using System.ComponentModel.DataAnnotations;

namespace MCComputersBackend.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public DateTime TransactionDate { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value")]
        public decimal? Discount { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one product must be specified")]
        public List<InvoiceProductDto> Products { get; set; } = new List<InvoiceProductDto>();
    }
}
