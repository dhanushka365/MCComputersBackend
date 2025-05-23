namespace MCComputersBackend.DTOs
{
    public class CreateInvoiceDto
    {
        public DateTime TransactionDate { get; set; }
        public decimal? Discount { get; set; }
        public List<InvoiceProductDto> Products { get; set; }
    }
}
