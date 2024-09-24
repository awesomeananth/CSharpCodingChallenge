using Microsoft.EntityFrameworkCore;
using RetailApp.Data;
using RetailApp.Dtos;
using RetailApp.Models;
using RetailApp.Services;

namespace RetailApp.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync(ProductSearchCriteria criteria)
        {
            var query = _context.Products.Where(p => p.Status == "Active").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query = query.Where(p => p.Name.Contains(criteria.Name));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice);
            }

            if (criteria.StartDate.HasValue)
            {
                query = query.Where(p => p.CreatedDate >= criteria.StartDate);
            }

            if (criteria.EndDate.HasValue)
            {
                query = query.Where(p => p.CreatedDate <= criteria.EndDate);
            }

            var products = await query.OrderByDescending(p => p.CreatedDate).ToListAsync();
            return new ServiceResponse<IEnumerable<Product>> { Data = products };
        }

        public async Task<ServiceResponse<Product>> CreateProductAsync(ProductDto productDto)
        {
            if (productDto.Price > 10000)
            {
                return new ServiceResponse<Product> { Success = false, Message = "Product price cannot exceed $10,000." };
            }

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Status = "Active",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            if (product.Price > 5000)
            {
                await AddToApprovalQueueAsync(product, "Create", "Price exceeds $5,000.");
            }
            else
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(int id, ProductDto productDto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return new ServiceResponse<Product> { Success = false, Message = "Product not found." };
            }

            decimal oldPrice = existingProduct.Price;
            decimal newPrice = productDto.Price;

            if (newPrice > 5000 || newPrice > 1.5m * oldPrice)
            {
                await AddToApprovalQueueAsync(existingProduct, "Update", "Price exceeds limits.");
            }
            else
            {
                existingProduct.Name = productDto.Name;
                existingProduct.Price = newPrice;
                existingProduct.UpdatedDate = DateTime.UtcNow;
                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }

            return new ServiceResponse<Product> { Data = existingProduct };
        }

        public async Task<ServiceResponse<Product>> DeleteProductAsync(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return new ServiceResponse<Product> { Success = false, Message = "Product not found." };
            }

            await AddToApprovalQueueAsync(existingProduct, "Delete", "Deletion requested.");

            return new ServiceResponse<Product> { Data = existingProduct };
        }

        private async Task AddToApprovalQueueAsync(Product product, string requestType, string reason)
        {
            var approvalItem = new ApprovalQueue
            {
                ProductId = product.ProductId,
                RequestType = requestType,
                RequestReason = reason,
                RequestDate = DateTime.UtcNow
            };
            _context.ApprovalQueue.Add(approvalItem);
            await _context.SaveChangesAsync();
        }
    }
}
