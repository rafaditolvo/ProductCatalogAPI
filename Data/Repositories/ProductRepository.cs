
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            return await _dbContext.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new ProductRepositoryException("Erro ao obter todos os produtos", ex);
        }
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        try
        {
            return await _dbContext.Products.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new ProductRepositoryException("Erro ao obter o produto por ID", ex);
        }
    }


    public async Task AddAsync(Product product)
    {
        try
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ProductRepositoryException("Erro ao adicionar o produto", ex);
        }
    }

    public async Task UpdateAsync(Product updatedProduct)
    {
        try
        {
            var product = await _dbContext.Products.FindAsync(updatedProduct.Id);

            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new ProductRepositoryException("Erro ao atualizar o produto", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new ProductRepositoryException("Erro ao excluir o produto", ex);
        }
    }
}

public class ProductRepositoryException : Exception
{
    public ProductRepositoryException()
    {
    }

    public ProductRepositoryException(string message) : base(message)
    {
    }

    public ProductRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
