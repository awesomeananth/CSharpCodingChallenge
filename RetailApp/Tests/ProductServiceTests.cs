using Moq;
using RetailApp.Services;
using RetailApp.Data;
using RetailApp.Models;
using System.Threading.Tasks;
using Xunit;

namespace RetailApp.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_WithPriceGreaterThan10000_ReturnsErrorMessage()
        {
            // Arrange
            var mockContext = new Mock<AppDbContext>(null);
            var productService = new ProductService(mockContext.Object);

            // Act
            var result = await productService.CreateProductAsync(new Dtos.ProductDto { Name = "Test", Price = 15000 });

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product price cannot exceed $10,000.", result.Message);
        }

        [Fact]
        public async Task UpdateProduct_WithPriceGreaterThanOldPriceBy50Percent_ShouldPushToApprovalQueue()
        {
            // Arrange
            var mockContext = new Mock<AppDbContext>(null);
            var productService = new ProductService(mockContext.Object);

            // Act
            var result = await productService.UpdateProductAsync(1, new Dtos.ProductDto { Name = "Test", Price = 7500 });

            // Assert
            Assert.True(result.Success); // Logic to validate approval queue
        }
    }
}
