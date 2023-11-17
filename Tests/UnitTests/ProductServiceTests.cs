using Xunit;
using Moq;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnProducts()
    {
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(new List<Product> { new Product { Id = 1, Name = "Dorflex" } });

        var mockRabbitMQService = new Mock<IRabbitMQService>();
        mockRabbitMQService.Setup(rabbitMQ => rabbitMQ.IsRabbitMQConnected())
                           .Returns(true);

        var productService = new ProductService(mockRepository.Object, mockRabbitMQService.Object);

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

        var mockRabbitMQService = new Mock<IRabbitMQService>();
        mockRabbitMQService.Setup(rabbitMQ => rabbitMQ.IsRabbitMQConnected())
                           .Returns(true);

        var productService = new ProductService(mockRepository.Object, mockRabbitMQService.Object);

        var result = await productService.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Product 1", result.Name);
    }

    [Fact]
    public async Task AddProductAsync_ShouldSendMessageToRabbitMQ()
    {
        var mockRepository = new Mock<IProductRepository>();
        var mockRabbitMQService = new Mock<IRabbitMQService>();
        mockRabbitMQService.Setup(rabbitMQ => rabbitMQ.IsRabbitMQConnected())
                           .Returns(true);

        var productService = new ProductService(mockRepository.Object, mockRabbitMQService.Object);

        var productToAdd = new Product { Id = 1, Name = "TestProduct" };

        await productService.AddProductAsync(productToAdd);

        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        mockRabbitMQService.Verify(rabbitMQ => rabbitMQ.SendMessage(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct()
    {

        var mockRepository = new Mock<IProductRepository>();
        var mockRabbitMQService = new Mock<IRabbitMQService>();
        mockRabbitMQService.Setup(rabbitMQ => rabbitMQ.IsRabbitMQConnected())
                           .Returns(true);

        var productService = new ProductService(mockRepository.Object, mockRabbitMQService.Object);

        var productId = 1;
        var updatedProduct = new Product { Id = productId, Name = "UpdatedProduct" };


        await productService.UpdateProductAsync(productId, updatedProduct);


        mockRepository.Verify(repo => repo.UpdateAsync(updatedProduct), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct()
    {

        var mockRepository = new Mock<IProductRepository>();
        var mockRabbitMQService = new Mock<IRabbitMQService>();
        mockRabbitMQService.Setup(rabbitMQ => rabbitMQ.IsRabbitMQConnected())
                           .Returns(true);

        var productService = new ProductService(mockRepository.Object, mockRabbitMQService.Object);

        var productId = 1;

        await productService.DeleteProductAsync(productId);


        mockRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
    }

}
