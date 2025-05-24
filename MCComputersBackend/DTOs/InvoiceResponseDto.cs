namespace MCComputersBackend.DTOs
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public List<InvoiceItemResponseDto> Items { get; set; } = new List<InvoiceItemResponseDto>();
    }
}
