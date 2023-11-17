using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        try
        {
            CheckRabbitMQConnection();

            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (RabbitMQNotConnectedException ex)
        {
            return StatusCode(503, $"Service Unavailable: RabbitMQ not connected - {ex.Message}");
        }
        catch (ProductServiceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        try
        {
            CheckRabbitMQConnection();

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }
        catch (RabbitMQNotConnectedException ex)
        {
            return StatusCode(503, $"Service Unavailable: RabbitMQ not connected - {ex.Message}");
        }
        catch (ProductServiceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Post([FromBody] Product product)
    {
        try
        {
            CheckRabbitMQConnection();

            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
        catch (RabbitMQNotConnectedException ex)
        {
            return StatusCode(503, $"Service Unavailable: RabbitMQ not connected - {ex.Message}");
        }
        catch (ProductServiceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Product updatedProduct)
    {
        try
        {
            CheckRabbitMQConnection();

            await _productService.UpdateProductAsync(id, updatedProduct);
            return NoContent();
        }
        catch (RabbitMQNotConnectedException ex)
        {
            return StatusCode(503, $"Service Unavailable: RabbitMQ not connected - {ex.Message}");
        }
        catch (ProductServiceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            CheckRabbitMQConnection();

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (RabbitMQNotConnectedException ex)
        {
            return StatusCode(503, $"Service Unavailable: RabbitMQ not connected - {ex.Message}");
        }
        catch (ProductServiceException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private void CheckRabbitMQConnection()
    {
        var rabbitMQService = _productService.GetRabbitMQService();

        if (rabbitMQService != null && !rabbitMQService.IsRabbitMQConnected())
        {
            throw new RabbitMQNotConnectedException("RabbitMQ is not connected.");
        }
    }
}
