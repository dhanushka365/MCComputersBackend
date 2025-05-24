using MCComputersBackend.Models;
using MCComputersBackend.Repository.Interface;
using MCComputersBackend.Service.Interface;

namespace MCComputersBackend.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _productRepository.GetProductsByCategoryAsync(category);
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            return await _productRepository.GetProductsByIdsAsync(ids);
        }
    }
}
