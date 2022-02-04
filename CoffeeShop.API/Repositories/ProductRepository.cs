using CoffeeShop.API.Data;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDbContext dbContext, ILogger<ProductRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Product> AddProduct(Product product)
    {
        try
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product added to DB.");
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not add Product to DB: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        try
        {
            var products = await _dbContext.Products.ToListAsync();
            _logger.LogInformation("Query 'products' was successful.");
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch products from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsForMenu(int menuId)
    {
        try
        {
            var products = await _dbContext.Products.Where(p => p.MenuId == menuId).ToListAsync();
            _logger.LogInformation("Query 'productsForMenu' was successful.");
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch products for menu with ID {menuId} from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<IdModel> RemoveProduct(int id)
    {
        try
        {
            var productObj = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            _dbContext.Products.Remove(productObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Product with ID {id} removed from DB.");
            var idObj = new IdModel { Id = productObj.Id };
            return idObj;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not remove Product with ID {id} from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<Product> UpdateProduct(int id, Product product)
    {
        try
        {
            var productObj = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            productObj.Name = product.Name;
            productObj.Description = product.Description;
            productObj.Price = product.Price;
            productObj.ImageUrl = product.ImageUrl;
            productObj.MenuId = product.MenuId;
            _dbContext.Products.Update(productObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Product with ID {id} updated in DB.");
            return productObj;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not update Reservation with ID {id} in DB: {ex.Message}");
            return null;
        }
    }
}