// ProductCatalogAPI/Test/UnitTests/ProductServiceTests.cs

using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnProducts()
    {
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(new List<Product> { new Product { Id = 1, Name = "Dorflex" } });

        var productService = new ProductService(mockRepository.Object);


        var result = await productService.GetAllProductsAsync();


        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct()
    {

        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => new Product { Id = id, Name = $"Product {id}" });

        var productService = new ProductService(mockRepository.Object);

        var result = await productService.GetProductByIdAsync(1);


        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Product 1", result.Name);
    }
}
