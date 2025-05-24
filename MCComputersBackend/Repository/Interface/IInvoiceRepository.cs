using MCComputersBackend.Models;

namespace MCComputersBackend.Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice);
        Task<Invoice?> GetByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetAllAsync();
    }
}
