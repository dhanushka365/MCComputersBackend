using MCComputersBackend.Models;
using MCComputersBackend.DTOs;

namespace MCComputersBackend.Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> ids);
        Task<Product> CreateProductAsync(CreateProductDto createProductDto);
        Task<Product?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
