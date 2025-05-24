using MCComputersBackend.DTOs;

namespace MCComputersBackend.Service.Interface
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto);
        Task<InvoiceResponseDto?> GetInvoiceAsync(int id);
        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
    }
}
