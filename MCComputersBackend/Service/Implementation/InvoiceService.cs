using MCComputersBackend.DTOs;
using MCComputersBackend.Models;
using MCComputersBackend.Repository.Interface;
using MCComputersBackend.Service.Interface;

namespace MCComputersBackend.Service.Implementation
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;
        private readonly IProductRepository _productRepo;
        
        public InvoiceService(IInvoiceRepository repo, IProductRepository productRepo)
        {
            _repo = repo;
            _productRepo = productRepo;
        }

        public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            // Validation
            if (dto.Products.Any(p => p.Quantity <= 0))
                throw new ArgumentException("Product quantity must be greater than zero.");

            var products = await _productRepo.GetProductsByIdsAsync(dto.Products.Select(p => p.ProductId).ToList());
            if (products.Count != dto.Products.Count)
                throw new ArgumentException("Some products do not exist.");

            decimal totalAmount = 0;
            var invoiceItems = new List<InvoiceItem>();
            
            foreach (var p in dto.Products)
            {
                var prod = products.First(x => x.Id == p.ProductId);
                totalAmount += prod.Price * p.Quantity;
                invoiceItems.Add(new InvoiceItem { ProductId = p.ProductId, Quantity = p.Quantity });
            }
            
            decimal discount = dto.Discount ?? 0;
            var balance = totalAmount - discount;

            var invoice = new Invoice
            {
                TransactionDate = dto.TransactionDate,
                Discount = discount,
                TotalAmount = totalAmount,
                BalanceAmount = balance,
                Items = invoiceItems
            };
            
            await _repo.AddAsync(invoice);
            
            return await MapToResponseDto(invoice, products);
        }

        public async Task<InvoiceResponseDto?> GetInvoiceAsync(int id)
        {
            var invoice = await _repo.GetByIdAsync(id);
            if (invoice == null) return null;

            var productIds = invoice.Items.Select(i => i.ProductId).ToList();
            var products = await _productRepo.GetProductsByIdsAsync(productIds);
            
            return await MapToResponseDto(invoice, products);
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
        {
            var invoices = await _repo.GetAllAsync();
            var result = new List<InvoiceResponseDto>();

            foreach (var invoice in invoices)
            {
                var productIds = invoice.Items.Select(i => i.ProductId).ToList();
                var products = await _productRepo.GetProductsByIdsAsync(productIds);
                result.Add(await MapToResponseDto(invoice, products));
            }

            return result;
        }

        private async Task<InvoiceResponseDto> MapToResponseDto(Invoice invoice, List<Product> products)
        {
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                TransactionDate = invoice.TransactionDate,
                Discount = invoice.Discount,
                TotalAmount = invoice.TotalAmount,
                BalanceAmount = invoice.BalanceAmount,
                Items = invoice.Items.Select(item =>
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return new InvoiceItemResponseDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        Quantity = item.Quantity,
                        TotalPrice = product.Price * item.Quantity
                    };
                }).ToList()
            };
        }
    }
}
