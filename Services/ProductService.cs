using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _productRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new ProductServiceException("Erro ao obter todos os produtos", ex);
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new ProductServiceException("Produto não encontrado", new Exception("Exceção interna: Produto não encontrado"));
            }

            return product;
        }
        catch (Exception ex)
        {
            throw new ProductServiceException("Erro ao obter o produto por ID", ex);
        }
    }

    public async Task AddProductAsync(Product product)
    {
        try
        {
            await _productRepository.AddAsync(product);
        }
        catch (Exception ex)
        {
            throw new ProductServiceException("Erro ao adicionar o produto", ex);
        }
    }

    public async Task UpdateProductAsync(int id, Product updatedProduct)
    {
        try
        {
            await _productRepository.UpdateAsync(updatedProduct);
        }
        catch (Exception ex)
        {
            throw new ProductServiceException("Erro ao atualizar o produto", ex);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        try
        {
            await _productRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new ProductServiceException("Erro ao excluir o produto", ex);
        }
    }
}

public class ProductServiceException : Exception
{
    public ProductServiceException()
    {
    }

    public ProductServiceException(string message) : base(message)
    {
    }

    public ProductServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

