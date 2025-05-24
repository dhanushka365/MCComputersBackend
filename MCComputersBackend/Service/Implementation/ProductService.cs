using MCComputersBackend.Models;
using MCComputersBackend.Repository.Interface;
using MCComputersBackend.Service.Interface;
using MCComputersBackend.DTOs;

namespace MCComputersBackend.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            
            // Set image URLs for each product
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    product.ImageUrl = _fileService.GetFileUrl(product.ImageFileName);
                }
            }
            
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            
            if (product != null && !string.IsNullOrEmpty(product.ImageFileName))
            {
                product.ImageUrl = _fileService.GetFileUrl(product.ImageFileName);
            }
            
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(category);
            
            // Set image URLs for each product
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    product.ImageUrl = _fileService.GetFileUrl(product.ImageFileName);
                }
            }
            
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> ids)
        {
            var products = await _productRepository.GetProductsByIdsAsync(ids);
            
            // Set image URLs for each product
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageFileName))
                {
                    product.ImageUrl = _fileService.GetFileUrl(product.ImageFileName);
                }
            }
            
            return products;
        }

        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                Category = createProductDto.Category,
                CreatedDate = DateTime.UtcNow
            };

            // Handle image upload
            if (createProductDto.Image != null && createProductDto.Image.Length > 0)
            {
                if (_fileService.IsValidImageFile(createProductDto.Image))
                {
                    var fileName = await _fileService.SaveFileAsync(createProductDto.Image, "products");
                    product.ImageFileName = fileName;
                    product.ImageUrl = _fileService.GetFileUrl(fileName);
                }
                else
                {
                    throw new ArgumentException("Invalid image file format or size");
                }
            }

            var createdProduct = await _productRepository.CreateProductAsync(product);
            
            // Set the image URL after creation
            if (!string.IsNullOrEmpty(createdProduct.ImageFileName))
            {
                createdProduct.ImageUrl = _fileService.GetFileUrl(createdProduct.ImageFileName);
            }
            
            return createdProduct;
        }

        public async Task<Product?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
                return null;

            // Update fields if provided
            if (!string.IsNullOrEmpty(updateProductDto.Name))
                existingProduct.Name = updateProductDto.Name;
            
            if (!string.IsNullOrEmpty(updateProductDto.Description))
                existingProduct.Description = updateProductDto.Description;
            
            if (updateProductDto.Price.HasValue)
                existingProduct.Price = updateProductDto.Price.Value;
            
            if (updateProductDto.StockQuantity.HasValue)
                existingProduct.StockQuantity = updateProductDto.StockQuantity.Value;
            
            if (!string.IsNullOrEmpty(updateProductDto.Category))
                existingProduct.Category = updateProductDto.Category;

            // Handle image update
            if (updateProductDto.Image != null && updateProductDto.Image.Length > 0)
            {
                if (_fileService.IsValidImageFile(updateProductDto.Image))
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingProduct.ImageFileName))
                    {
                        await _fileService.DeleteFileAsync(existingProduct.ImageFileName);
                    }

                    // Save new image
                    var fileName = await _fileService.SaveFileAsync(updateProductDto.Image, "products");
                    existingProduct.ImageFileName = fileName;
                    existingProduct.ImageUrl = _fileService.GetFileUrl(fileName);
                }
                else
                {
                    throw new ArgumentException("Invalid image file format or size");
                }
            }

            existingProduct.UpdatedDate = DateTime.UtcNow;
            var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);
            
            // Set the image URL after update
            if (!string.IsNullOrEmpty(updatedProduct.ImageFileName))
            {
                updatedProduct.ImageUrl = _fileService.GetFileUrl(updatedProduct.ImageFileName);
            }
            
            return updatedProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return false;

            // Delete associated image file
            if (!string.IsNullOrEmpty(product.ImageFileName))
            {
                await _fileService.DeleteFileAsync(product.ImageFileName);
            }

            return await _productRepository.DeleteProductAsync(id);
        }
    }
}
