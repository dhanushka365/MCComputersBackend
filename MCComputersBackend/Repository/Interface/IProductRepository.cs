using MCComputersBackend.Models;

namespace MCComputersBackend.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<List<Product>> GetProductsByIdsAsync(List<int> ids);
    }
}
