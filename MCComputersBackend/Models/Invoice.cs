namespace MCComputersBackend.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public List<InvoiceItem> Items { get; set; }
    }
}
